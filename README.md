# 🍽️ Restaurant Reservation API

The **Restaurant Reservation API** is a modern, modular, and secure web API built with **.NET 9** and **Minimal APIs** for managing restaurant operations.  
It provides endpoints for customers, employees, reservations, restaurants, tables, menu items, orders, and order items — complete with **JWT authentication**, **FluentValidation**, **AutoMapper**, and **Swagger/OpenAPI** documentation.

---

## 🌟 Key Features
- 🔐 **JWT Authentication** – Secure access for all API endpoints.
- 🧾 **Full CRUD Operations** – Manage all restaurant entities with ease.
- ✅ **FluentValidation Integration** – Reusable validation for DTOs.
- 🔄 **AutoMapper Profiles** – Simplified mapping between models and DTOs.
- ⚙️ **Layered Architecture** – Core, Db, API, and Shared DTOs.
- 📊 **Entity Framework Core** – Handles data persistence.
- 🧠 **Exception Handling Middleware** – Uniform API error responses.
- 📚 **Swagger UI** – Interactive documentation.

---

## 🧩 Technologies Used

| Category | Tools / Frameworks |
|-----------|--------------------|
| Backend Framework | **.NET 9**, Minimal APIs |
| Authentication | **JWT (JSON Web Tokens)** |
| ORM / Database | **Entity Framework Core** |
| Validation | **FluentValidation** |
| Object Mapping | **AutoMapper** |
| API Docs | **Swagger / Swashbuckle** |
| Language | **C#** |

---

## 🏗️ Project Structure
```
RestaurantReservation/
├── API/                    # Minimal API layer (Endpoints, Auth, Program.cs)
├── Core/                   # Business logic & services
├── Db/                     # Models & Repositories
├── Shared/                 # Shared DTOs between layers
└── ConsoleApp/             # Optional CLI interface
```

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/<your-username>/RestaurantReservation.git
cd RestaurantReservation
```

### 2. Configure environment
Create an `appsettings.json` in the API project:
```json
{
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "RestaurantReservationAPI",
    "Audience": "RestaurantReservationClient"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-db-connection-string"
  }
}
```

### 3. Run the project
```bash
dotnet run --project RestaurantReservation.API
```
Swagger UI: **https://localhost:5001/swagger**

---

## 🔑 Authentication Example

**Request:**
```bash
POST /api/auth/token
Content-Type: application/json

{
  "username": "admin",
  "email": "admin@example.com"
}
```

**Response:**
```json
{
  "token": "<JWT_TOKEN>",
  "expiresOn": "2025-11-02T15:00:00Z"
}
```

---

## 🧠 Example Endpoints

| Resource | Method | Route | Description |
|-----------|---------|-------|-------------|
| Customers | GET | `/api/customers` | Get all customers |
| Employees | POST | `/api/employees` | Add new employee |
| Reservations | GET | `/api/reservations/{id}` | Get reservation by ID |
| Orders | PUT | `/api/orders/{id}` | Update an order |
| Tables | DELETE | `/api/tables/{id}` | Delete a table |

---

## 🧩 Validation & Error Handling
- **400 Bad Request:** Invalid or missing data fields  
- **404 Not Found:** Entity doesn’t exist  
- **401 Unauthorized:** Missing/Invalid JWT token  
- **500 Internal Server Error:** Unexpected exception handled gracefully  

---

## 🧑‍💻 Author
**Ahmad Hamdan**  
📍 Birzeit University – Computer Engineering Student  
---
