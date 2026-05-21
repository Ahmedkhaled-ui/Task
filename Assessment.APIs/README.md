# Project & Task Management System API

A production-ready Web API built with **.NET 9** implementing **Clean Architecture** principles to manage Projects and Tasks dynamically. The system includes built-in JWT authentication, robust data validation, and a centralized corporate-level response formatting system.

---

## 🏗️ Architecture & Enterprise Patterns

This project strictly adheres to production-grade backend engineering standards:

* **Clean Architecture (Onion Pattern):** Complete separation of concerns between Core (Domain & Application), Infrastructure (Data Access), and Presentation (Web API) layers. This ensures the business logic is entirely independent of frameworks, databases, or UI.
* **Repository & Unit of Work Patterns:** Decouples the application layer from Entity Framework Core 9. All database operations across multiple repositories are executed under a single transaction context to guarantee data integrity.
* **Global Exception Handling Middleware:** Intercepts all runtime exceptions globally. It maps specific domain/business exceptions to their respective HTTP Status Codes (e.g., `KeyNotFoundException` to `404 Not Found`) without exposing sensitive stack traces in production.
* **Unified API Response Wrapper (`ApiResponse<T>`):** All endpoints (including success and error states handled by the middleware) adhere to a rigid, predictable JSON structure, enhancing the integration experience for Frontend clients (Angular/React).

---

## 🛠️ Tech Stack & Key Dependencies

* **Framework:** .NET 9.0 (Web API)
* **ORM:** Entity Framework Core 9
* **Database:** Microsoft SQL Server
* **Identity & Security:** Microsoft Identity Core with JWT Bearer Authentication
* **API Documentation:** OpenAPI / Scalar API Reference

---

## 📊 Unified Response Contract

Every single API response returns the following standardized JSON format:

### Success Response Example (200 OK / 201 Created)
```json
{
  "success": true,
  "message": "Project retrieved successfully.",
  "data": {
    "id": 1,
    "name": "E-Commerce Store Project",
    "description": "Minimalist luxury product platform",
    "createdAt": "2026-05-20T21:00:00Z"
  }
}