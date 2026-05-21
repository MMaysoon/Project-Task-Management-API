# 📘 Project & Task Management API

## 📌 Introduction

This is a backend REST API built using **ASP.NET Core (.NET 9)**.

It implements **Clean Architecture**, **JWT Authentication**, and applies **Dependency Injection**, **CQRS (MediatR)**, and **Service-based design**.

The system allows authenticated users to manage Projects and Tasks securely, where each user can access only their own data.


---

## 🚀 Setup Instructions

### 📥 Clone Repository
```bash
git clone https://github.com/MMaysoon/Project-Task-Management-API.git
cd Project-Task-Management-API
```

### ⚙️ Configure Database

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ProjectTaskDB;Trusted_Connection=True;"
}
```

Make sure SQL Server is running locally.

### 🧱 Apply Migrations

```bash
dotnet ef database update
```

### ▶️ Run Application

```bash
dotnet run
```

Or run via Visual Studio.

### 📖 Swagger

After running the project:

```
https://localhost:{port}/swagger
```

---

## 🏗️ Architecture Overview

The project follows **Clean Architecture** with separation between layers to ensure scalability and maintainability.

### 📌 Project Structure

```
ProjectTaskManagementAPI
│
├── API Layer
│   ├── Controllers
│   ├── Middleware (Global Exception Handling)
│   ├── Program.cs
│
├── Application Layer
│   ├── helpers
│       ├── JWT
│   ├── DTOS
│   ├── Features
│   │   ├── Projects
│   │       ├── Commands
│   │       ├── Queries
│   ├── Interfaces
│   ├── Common (Responses, Exceptions)
│   ├── IServices
│   ├── Services
│   ├── Mappings
│
├── Domain Layer
│   ├── Entities
│   │   ├── User
│   │   ├── Project
│   │   ├── Task
│   │
│   ├── Enums (Status, Priority)
│
├── Infrastructure Layer
│   ├── Data
│     ├── ApplicationDbContext
│     ├──  Seed 
│        ├── RoleSeeder
│   ├── Migrations

```
## 🔁 Request Flow

### For Service-based endpoints (Tasks, Auth, remaining CRUD):

```
Controller → Service Layer (Dependency Injection) → Repository/DbContext → Response
```

### For CQRS-based endpoints (Project module - Create & GetAll only):

```
Controller → MediatR → Handler → DbContext → Response DTO
```

---

## 🧠 Implementation Details

### 🔹 Dependency Injection (DI)

The project is fully built using Dependency Injection with Service Layer pattern:

- Services are injected into controllers via interfaces
- Loose coupling between layers is maintained
- Business logic is separated from controllers

**Example:**
```csharp
IProjectService → ProjectService
ITaskService → TaskService
```

### 🔹 CQRS + MediatR (Partial Implementation)

CQRS pattern is applied **only in selected endpoints**:

**Implemented in Project Module:**
- ✅ Create Project → Command + MediatR
- ✅ Get All Projects → Query + MediatR

These endpoints follow:
- Command / Query separation
- Handlers for each operation
- DTO-based responses

### 🔹 Remaining Endpoints

Other endpoints (Tasks module, Update/Delete Project, etc.) are implemented using **Service-based architecture** (without MediatR) for simplicity and clarity.

### 🔹 Global Exception Handling (Middleware)

A centralized exception handling middleware is implemented.

**What it does:**
- Catches all unhandled exceptions globally
- Prevents application crashes
- Returns consistent API error responses
- Removes need for try/catch in controllers

**Example response:**
```json
{
  "success": false,
  "message": "An unexpected error occurred",
  "statusCode": 500
}
```

---

## 🔐 Authentication & Authorization

### Authentication (JWT)

- Users register and login
- Passwords are securely hashed
- JWT token contains **UserId** and **Role claims**
- Token required for all protected endpoints

### Authorization (Role-Based)

| Role | Permissions |
|------|-------------|
| **User** | Can access and manage only their own projects and tasks |
| **Admin (if seeded)** | Has full access to all data |

---

## 📁 Modules

### 🔹 Auth Module

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login and get JWT token |

### 🔹 Project Module

| Method | Endpoint | Pattern |
|--------|----------|---------|
| POST | `/api/projects` | **CQRS + MediatR** (Create Project) |
| GET | `/api/projects` | **CQRS + MediatR** (Get All Projects) |
| GET | `/api/projects/{id}` | Service-based |
| PUT | `/api/projects/{id}` | Service-based |
| DELETE | `/api/projects/{id}` | Service-based |

**Rules:**
- Each project belongs to a user
- Users can only access their own projects

### 🔹 Task Module (All Service-based)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/tasks` | Create task |
| GET | `/api/tasks/project/{projectId}` | Get tasks by project |
| PUT | `/api/tasks/status` | Update task status |
| DELETE | `/api/tasks/{id}` | Delete task |

**Fields:**
- Title
- Description
- Status (Pending, InProgress, Completed)
- Priority (Low, Medium, High)
- DueDate
- ProjectId

---

## 🧾 Database Schema

### User
| Column | Type |
|--------|------|
| Id | int (PK) |
| Username | string |
| Email | string |
| PasswordHash | string |
| Role | string |

### Project
| Column | Type |
|--------|------|
| Id | int (PK) |
| Name | string |
| Description | string |
| CreatedAt | DateTime |
| UserId | int (FK → User) |

### Task
| Column | Type |
|--------|------|
| Id | int (PK) |
| Title | string |
| Description | string |
| Status | Enum |
| Priority | Enum |
| DueDate | DateTime |
| ProjectId | int (FK → Project) |

---

## 📌 Key Highlights

| Feature | Status |
|---------|--------|
| Dependency Injection across all services | ✅ |
| Service Layer architecture (most modules) | ✅ |
| CQRS + MediatR (Project: Create + GetAll only) | ✅ |
| Global Exception Handling middleware | ✅ |
| Clean Architecture strictly followed | ✅ |
| DTO pattern for request/response separation | ✅ |
| JWT Authentication & Role-based Authorization | ✅ |
