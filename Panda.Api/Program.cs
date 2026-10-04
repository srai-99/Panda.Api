using Microsoft.EntityFrameworkCore;
using Panda.Api.Models.Requests.Appointment;
using Panda.Api.Models.Requests.Patient;
using Panda.Api.Validators.Appointment;
using Panda.Api.Validators.Patient;
using Panda.Application.Services.Appointment;
using Panda.Application.Validators;
using Panda.Domain.Entities;
using Panda.Domain.Enums;
using Panda.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PandaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", () => Results.Redirect("/swagger"))
       .ExcludeFromDescription();
}

// Patients endpoints
app.MapPost("/patients", async (PatientCreateModel request, PandaDbContext db) =>
{
    try
    {
        PatientRequestValidator.EnsureValid(request);

        NhsNumberValidator.EnsureValid(request.NhsNumber);

        var patient = new Patient
        {
            NhsNumber = request.NhsNumber,
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Postcode = PostcodeValidator.Normalise(request.Postcode)
        };

        db.Patients.Add(patient);
        await db.SaveChangesAsync();

        return Results.Created($"/patients/{patient.NhsNumber}", patient);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "VALIDATION_ERROR",
            Message = ex.Message
        });
    }
    catch (DbUpdateException dbEx)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "DATABASE_ERROR",
            Message = $"Failed to save patient to the database. {dbEx.InnerException?.Message ?? dbEx.Message}"
        });
    }
});

app.MapGet("/patients/{nhsNumber}", async (string nhsNumber, PandaDbContext db) =>
{
    var patient = await db.Patients.FindAsync(nhsNumber);
    return patient is null
        ? Results.NotFound(new { ErrorCode = "NOT_FOUND", Message = "Patient not found." })
        : Results.Ok(patient);
});

app.MapPatch("/patients/{nhsNumber}", async (string nhsNumber, PatientUpdateModel request, PandaDbContext db) =>
{
    var patient = await db.Patients.FindAsync(nhsNumber);
    if (patient is null)
        return Results.NotFound(new { ErrorCode = "NOT_FOUND", message = "Patient not found." });

    try
    {
        PatientUpdateValidator.EnsureValid(request);

        if (!string.IsNullOrWhiteSpace(request.Name))
            patient.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Postcode))
            patient.Postcode = PostcodeValidator.Normalise(request.Postcode);

        await db.SaveChangesAsync();
        return Results.Ok(patient);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "VALIDATION_ERROR",
            Message = ex.Message
        });
    }
    catch (DbUpdateException dbEx)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "DATABASE_ERROR",
            Message = $"Failed to save patient to the database. {dbEx.InnerException?.Message ?? dbEx.Message}"
        });
    }
});

app.MapDelete("/patients/{nhsNumber}", async (string nhsNumber, PandaDbContext db) =>
{
    var patient = await db.Patients.FindAsync(nhsNumber);
    if (patient is null)
        return Results.NotFound(new { ErrorCode = "NOT_FOUND", Message = "Patient not found." });

    db.Patients.Remove(patient);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Appointments endpoints
app.MapPost("/appointments", async (AppointmentCreateModel request, PandaDbContext db, IAppointmentService svc) =>
{

    try
    {
        AppointmentCreateValidator.EnsureValid(request);

        var patient = await db.Patients.FindAsync(request.Patient);
        if (patient is null)
        {
            return Results.BadRequest(new
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Patient does not exist."
            });
        }

        var statusEnum = Enum.Parse<AppointmentStatus>(request.Status, ignoreCase: true);

        if (statusEnum != AppointmentStatus.Active)
            return Results.BadRequest(new
            {
                ErrorCode = "VALIDATION_ERROR",
                Message = "Only new and active appointments can be created."
            });

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientNhsNumber = request.Patient,
            Status = statusEnum,
            Time = request.Time,
            Duration = request.Duration,
            Clinician = request.Clinician,
            Department = request.Department,
            Postcode = PostcodeValidator.Normalise(request.Postcode)
        };

        svc.ApplyBusinessRules(appointment);

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();

        return Results.Created($"/appointments/{appointment.Id}", appointment);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "VALIDATION_ERROR",
            Message = ex.Message
        });
    }
    catch (DbUpdateException dbEx)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "DATABASE_ERROR",
            Message = $"Failed to save appointment to the database. {dbEx.InnerException?.Message ?? dbEx.Message}"
        });
    }
});

app.MapGet("/appointments/{id}", async (Guid id, PandaDbContext db) =>
{
    var appt = await db.Appointments.FindAsync(id);
    if (appt is null)
        return Results.NotFound(new
        {
            ErrorCode = "NOT_FOUND",
            Message = "Appointment not found."
        });

    return Results.Ok(appt);
});

app.MapPatch("/appointments/{id}", async (Guid id, AppointmentUpdateModel request, PandaDbContext db, IAppointmentService svc) =>
{
    var appt = await db.Appointments.FindAsync(id);
    if (appt is null)
        return Results.NotFound(new { ErrorCode = "NOT_FOUND", Message = "Appointment not found." });

    try
    {
        AppointmentUpdateValidator.EnsureValid(request);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var newStatus = Enum.Parse<AppointmentStatus>(request.Status, ignoreCase: true);

            if (newStatus == AppointmentStatus.Cancelled)
            {
                svc.Cancel(appt);
            }
            else
            {
                if (appt.Status == AppointmentStatus.Cancelled)
                    throw new ValidationException("Cannot change the status of a cancelled appointment.");

                appt.Status = newStatus;
            }
        }

        if (request.Time.HasValue)
            appt.Time = request.Time.Value;

        svc.ApplyBusinessRules(appt);

        if (!string.IsNullOrWhiteSpace(request.Clinician))
            appt.Clinician = request.Clinician;

        if (!string.IsNullOrWhiteSpace(request.Department))
            appt.Department = request.Department;

        if (!string.IsNullOrWhiteSpace(request.Postcode))
            appt.Postcode = PostcodeValidator.Normalise(request.Postcode);

        await db.SaveChangesAsync();
        return Results.Ok(appt);
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "RULE_VIOLATION",
            Message = ex.Message
        });
    }
    catch (DbUpdateException dbEx)
    {
        return Results.BadRequest(new
        {
            ErrorCode = "DATABASE_ERROR",
            Message = $"Failed to update appointment. {dbEx.InnerException?.Message ?? dbEx.Message}"
        });
    }
});

app.Run();