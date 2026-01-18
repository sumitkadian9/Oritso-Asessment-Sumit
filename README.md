# Task Management System - Oritso Technical Assessment

## Overview
This is a Full-Stack Task Management application designed using .NET and Angular. The application features a decoupled architecture with a RESTful API backend and a responsive Single Page Application (SPA) frontend. It supports full CRUD operations, authentication, and real-time search.

## DB Design

### ER Diagram
The system uses a 1-to-Many relationship between Users and Tasks.

    ```mermaid
    erDiagram
    USER ||--o{ TODOTASK : "creates"
    USER {
        guid Id PK "Primary Key"
        string Email UK "Unique Login Identifier"
        string FirstName "User's First Name"
        string LastName "User's Last Name"
        string PasswordHash "BCrypt Hashed Password"
        string PasswordSalt "Unique Salt for Hashing"
        int Role "Enum: 0-Admin, 1-Member"
    }
    TODOTASK {
        guid Id PK "Primary Key"
        string Title "Task Headline"
        string Description "Detailed Task Info"
        date DueDate "Expected Completion Date"
        int Status "Enum: 0-Pending, 1-InProgress, 2-Completed"
        string Remarks "Additional Notes"
        guid UserId FK "Foreign Key to User Table"
        long CreatedOn "Creation Unix Timestamp"
        long LastUpdatedOn "Modified Unix Timestamp"
    }
    ```


### Indexes

Primary Keys: On User.Id and TodoTask.Id.
Unique Index: Applied to User.Email to ensure non-duplicate sign ups.
Foreign Key Index: Applied to TodoTask.UserId for relational operations.

### Approach
I utilized a code-first approach using EF Core as an ORM. This ensures the Domain Model is the source of truth.

### Structure Of Application
The project utilizes Single Page Application (SPA) with API Binding

The project uses a decoupled SPA architecture:
Backend: ASP.NET Core 9.0 Web API (REST).
 -- Backend has N-tier architecture with presentation layer (controllers), business layer(services), and data layer (ef dbContext)
 -- Authentication/Authorization via bearer scheme using JWT tokens
 -- Key dependencies: FluentResults (for result pattern), NSwag (for swagger UI and openAPI specs), Pomelo.EntityFrameworkCore.MySQL for connecting to MySql Database.

Frontend: Angular 21.
 -- Binding: The frontend communicates with the backend via asynchronous HTTP calls using Angular's HttpClient, secured with JWT.
 -- Styling: Tailwind was used to rapidly create responsive UI pages.

Database: MySQL 8.0.31

### Instructions to Compile/Build
Database: Ensure MySQL is running and update connection in appsettings.json.
To build the project follow these instructions (In command prompt or terminal or similar):

For backend:
cd TaskManagement.API
dotnet restore
dotnet ef database update
dotnet build

For frontend:
cd TaskManagement.Web
npm install
npm run build


### Instructions to Run
Run Backend: dotnet run (Usually https://localhost:7143)
Run Frontend: ng serve (Usually http://localhost:4200)
Seeded Data: Use 'admin@tmapp.com' with password 'admin' to log in immediately, or you can register a new account.

Start the backend API project first and then the frontend project :
1. Start the API: `cd TaskManagement.API && dotnet run`
2. Start the Frontend: `cd TaskManagement.Web && npm start`
