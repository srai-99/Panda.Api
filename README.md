# Panda API

A simple healthcare API for managing Patients and Appointments.

## Running the API

1. Open the solution:
   ```
   Panda.Api.sln
   ```

2. Run the project:
   ```
   Panda.Api
   ```

3. Swagger UI will be available at:
   ```
   https://localhost:7291/swagger/index.html
   ```

Base URL:
```
https://localhost:7291
```

## Patients API

- `POST /patients` – Create a patient  
- `GET /patients/{nhsNumber}` – Get a patient  
- `PATCH /patients/{nhsNumber}` – Update a patient  
- `DELETE /patients/{nhsNumber}` – Delete a patient  

## Appointments API

- `POST /appointments` – Create an appointment  
- `GET /appointments/{id}` – Get an appointment  
- `PATCH /appointments/{id}` – Update an appointment  

## Business Rules

- Only Active appointments can be created  
- Cancelled appointments cannot be modified  
- Missed appointments cannot be cancelled  
- Attended appointments cannot be cancelled  
- Appointments become Missed automatically when the end time is in the past

## Testing

Unit tests use:
- NUnit  
- FluentAssertions  
