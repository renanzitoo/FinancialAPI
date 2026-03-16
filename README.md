# 💰 FinancialAPI

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)](https://www.mysql.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A RESTful API for **personal finance management** with JWT authentication, transaction tracking, and category organization.

---

## 📋 About the Project

FinancialAPI allows users to register, authenticate, and manage their personal finances by creating income and expense transactions organized into custom categories. Each user's data is fully isolated.

### 🎯 Features

- 🔐 **JWT Authentication** — secure registration and login
- 🗂️ **Categories** — create and manage custom transaction categories
- 💸 **Transactions** — record income and expenses in cents
- 📊 **Summaries** — overall and monthly financial summaries
- 🔍 **Filters** — query transactions by date range or category
- 📖 **Swagger/OpenAPI** — interactive API documentation

---

## 🛠️ Technologies Used

- [x] **.NET 8.0** — Web framework
- [x] **ASP.NET Core Web API** — RESTful API construction
- [x] **Entity Framework Core 8.0** — ORM for data access
- [x] **Pomelo MySQL 8.0** — MySQL EF Core provider
- [x] **AutoMapper 12.0** — Object-to-object mapping
- [x] **FluentValidation 11** — Input validation
- [x] **JWT Bearer** — Stateless authentication
- [x] **Swagger/OpenAPI** — API documentation

---

## 📁 Project Structure

```
FinancialAPI/
├── Controllers/
│   ├── AuthController.cs         # Register & login
│   ├── CategoryController.cs     # Category CRUD
│   └── TransactionController.cs  # Transaction CRUD & summaries
├── Services/
│   ├── CategoryService.cs
│   ├── TransactionService.cs
│   ├── JwtService.cs
│   ├── PasswordService.cs
│   └── CurrentUserService.cs
├── Entities/
│   ├── User.cs
│   ├── Category.cs
│   └── Transaction.cs            # TransactionType enum (Income/Expense)
├── DTOs/
│   ├── Requests/                 # Auth, Category, Transaction DTOs
│   └── Responses/
├── Interfaces/
│   ├── ICategoryService.cs
│   ├── ITransactionService.cs
│   └── ICurrentUserService.cs
├── Context/
│   └── AppDbContext.cs
├── Mappings/
│   ├── CategoryMapping.cs
│   └── TransactionMapping.cs
└── Migrations/
```

---

## 💻 Requirements

- **.NET SDK 8** or higher
- **MySQL 8.0** or Docker
- **Git**
- IDE: **JetBrains Rider**, **Visual Studio 2022**, or **VS Code**

---

## 🚀 Running the Project

### 1️⃣ Clone the repository

```bash
git clone https://github.com/renanzitoo/FinancialAPI.git
cd FinancialAPI
```

### 2️⃣ Configure the database and JWT secret

Edit `FinancialAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=financialapi;User=root;Password=your-password;"
  },
  "Jwt": {
    "Secret": "your-secret-key-at-least-32-characters-long"
  }
}
```

#### Option: MySQL via Docker

```bash
docker run --name mysql-financialapi -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=financialapi -p 3306:3306 -d mysql:8.0
```

### 3️⃣ Run the migrations

```bash
dotnet ef database update --project FinancialAPI
```

### 4️⃣ Restore and run

```bash
dotnet restore
dotnet run --project FinancialAPI
```

API available at: `https://localhost:7000` or `http://localhost:5000`

---

## 📌 API Endpoints

All endpoints except **Auth** require a `Bearer` token in the `Authorization` header.

### 🔐 Auth

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Create a new account |
| POST | `/api/auth/login` | Authenticate and receive a JWT |

### 🗂️ Categories

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/categories` | Create a category |
| GET | `/api/categories` | List all user categories |
| GET | `/api/categories/{id}` | Get a category by ID |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category |

### 💸 Transactions

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/transactions` | Create a transaction |
| GET | `/api/transactions` | List all user transactions |
| GET | `/api/transactions/{id}` | Get a transaction by ID |
| PUT | `/api/transactions/{id}` | Update a transaction |
| DELETE | `/api/transactions/{id}` | Delete a transaction |
| GET | `/api/transactions/by-date` | Filter by date range (`?startDate=&endDate=`) |
| GET | `/api/transactions/by-category/{categoryId}` | Filter by category |
| GET | `/api/transactions/summary` | Overall financial summary |
| GET | `/api/transactions/summary/{year}/{month}` | Monthly financial summary |

---

## 📝 Example Requests

### Register
```bash
POST /api/auth/register
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "P@ssw0rd!"
}
```

### Create Transaction
```bash
POST /api/transactions
Authorization: Bearer <token>
{
  "title": "Salary",
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amountInCents": 500000,
  "type": 1,
  "description": "Monthly salary",
  "date": "2026-03-16T00:00:00Z"
}
```
> `type`: `1` = Income, `2` = Expense. `amountInCents`: amount in the smallest currency unit (e.g. 500000 = $5,000.00).

---

## 🗄️ Data Model

### `Users`
| Column | Type |
|--------|------|
| Id | GUID |
| Name | VARCHAR |
| Email | VARCHAR(100) |
| PasswordHash | VARCHAR |
| CreatedAt | DATETIME |

### `Categories`
| Column | Type |
|--------|------|
| Id | GUID |
| Name | VARCHAR |
| UserId | GUID (FK) |

### `Transactions`
| Column | Type |
|--------|------|
| Id | GUID |
| UserId | GUID (FK) |
| CategoryId | GUID (FK) |
| Title | VARCHAR |
| AmountInCents | BIGINT |
| Description | VARCHAR |
| Date | DATETIME |
| Type | INT (1=Income, 2=Expense) |

---

## 🏗️ Architecture

### Implemented Patterns

1. **Service Layer** — Business logic isolated in service classes
2. **DTO Pattern** — Separation of entities and API models
3. **Interface Segregation** — Services registered via interfaces
4. **Dependency Injection** — Native ASP.NET Core DI
5. **AutoMapper Profiles** — Clean entity ↔ DTO conversion

---

## 🧠 Concepts Practiced

- ✅ External API consumption with `HttpClient`
- ✅ Resilience and error handling
- ✅ Local caching for optimization
- ✅ Async/Await patterns
- ✅ Entity Framework Core with MySQL
- ✅ AutoMapper for DTOs
- ✅ Data validation
- ✅ Call auditing
- ✅ RESTful patterns
- ✅ Dependency injection

---

## 📝 Future Improvements

- [ ] Add `ILogger` for structured observability
- [ ] Implement Circuit Breaker with Polly
- [ ] Create unit tests (xUnit)
- [ ] Integration tests
- [ ] Health checks
- [ ] Metrics (Prometheus)
- [ ] Containerize the application
- [ ] CI/CD with GitHub Actions
- [ ] Rate limiting

---

## 👨‍💻 Author

**Renan Costa**  
GitHub: [renanzitoo](https://github.com/renanzitoo)

---

**⭐ If this project was useful to you, consider giving it a star!**
