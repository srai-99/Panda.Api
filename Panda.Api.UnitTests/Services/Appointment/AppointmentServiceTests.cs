using FluentAssertions;
using Panda.Application.Services.Appointment;
using Panda.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Panda.Api.UnitTests.Services.Appointment
{

    [TestFixture]
    public class AppointmentServiceTests
    {
        private AppointmentService service = null!;

        [SetUp]
        public void Setup()
        {
            service = new AppointmentService();
        }

        [Test]
        public void ApplyBusinessRules_ShouldNotChangeStatus_WhenCancelled()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment
            {
                Status = AppointmentStatus.Cancelled,
                Duration = "15m",
                Time = DateTimeOffset.UtcNow.AddMinutes(-30)
            };

            // Act
            service.ApplyBusinessRules(appt);

            // Assert
            appt.Status.Should().Be(AppointmentStatus.Cancelled);
        }

        [Test]
        public void ApplyBusinessRules_ShouldNotChangeStatus_WhenMissed()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment
            {
                Status = AppointmentStatus.Missed,
                Duration = "15m",
                Time = DateTimeOffset.UtcNow.AddMinutes(-30)
            };

            // Act
            service.ApplyBusinessRules(appt);

            // Assert
            appt.Status.Should().Be(AppointmentStatus.Missed);
        }

        [Test]
        public void ApplyBusinessRules_ShouldMarkAppointmentAsMissed_WhenEndTimeIsPast()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment
            {
                Status = AppointmentStatus.Active,
                Duration = "15m",
                Time = DateTimeOffset.UtcNow.AddMinutes(-30)
            };

            // Act
            service.ApplyBusinessRules(appt);

            // Assert
            appt.Status.Should().Be(AppointmentStatus.Missed);
        }

        [Test]
        public void ApplyBusinessRules_ShouldNotMarkAsMissed_WhenEndTimeIsFuture()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment
            {
                Status = AppointmentStatus.Active,
                Duration = "15m",
                Time = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            // Act
            service.ApplyBusinessRules(appt);

            // Assert
            appt.Status.Should().Be(AppointmentStatus.Active);
        }

        [Test]
        public void Cancel_ShouldThrow_WhenAlreadyCancelled()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment { Status = AppointmentStatus.Cancelled };

            // Act
            Action act = () => service.Cancel(appt);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Appointment is already cancelled.");
        }

        [Test]
        public void Cancel_ShouldThrow_WhenMissed()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment { Status = AppointmentStatus.Missed };

            // Act
            Action act = () => service.Cancel(appt);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Missed appointments cannot be cancelled.");
        }

        [Test]
        public void Cancel_ShouldThrow_WhenAttended()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment { Status = AppointmentStatus.Attended };

            // Act
            Action act = () => service.Cancel(appt);

            // Assert
            act.Should()
                .Throw<ValidationException>()
                .WithMessage("Attended appointments cannot be cancelled.");
        }

        [Test]
        public void Cancel_ShouldSetStatusToCancelled_WhenActive()
        {
            // Arrange
            var appt = new Domain.Entities.Appointment { Status = AppointmentStatus.Active };

            // Act
            service.Cancel(appt);

            // Assert
            appt.Status.Should().Be(AppointmentStatus.Cancelled);
        }
    }
}