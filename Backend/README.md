# BeSpoked Bikes — Backend API

.NET 8 Web API with Entity Framework Core and SQLite for tracking bike sales, salesperson commissions, and inventory.

## Setup

```bash
dotnet restore
dotnet ef database update
dotnet run
```

API runs at `http://localhost:5194` with Swagger UI at `/swagger`.

## Project Structure

```
Backend/
├── Controllers/        # API endpoints
├── Models/             # EF Core entity classes
├── DTOs/               # Request/response records
├── Data/               # DbContext, seed data
├── Services/           # Business logic (sale creation)
├── Migrations/         # EF Core migrations
├── Program.cs          # App startup and DI config
└── appsettings.json    # Connection string, logging
```

## API Endpoints

| Method | Route                          | Description                          |
|--------|--------------------------------|--------------------------------------|
| GET    | /api/products                  | List all products                    |
| GET    | /api/products/{id}             | Get single product                   |
| PUT    | /api/products/{id}             | Update product (409 on duplicate)    |
| GET    | /api/salespersons              | List all salespersons                |
| GET    | /api/salespersons/{id}         | Get single salesperson               |
| PUT    | /api/salespersons/{id}         | Update salesperson (409 on duplicate)|
| GET    | /api/customers                 | List all customers                   |
| GET    | /api/sales?startDate=&endDate= | List sales with optional date filter |
| POST   | /api/sales                     | Create a new sale                    |
| GET    | /api/sales/quarterly-report    | Commission report (?year=&quarter=)  |
| GET    | /api/discounts                 | List all discounts                   |

## Key Design Decisions

- **Controller-based API** — standard pattern, easy to follow and extend.
- **SaleService** — business logic (discount lookup, commission calc, stock check) lives in a service, not the controller.
- **DTOs as records** — immutable data contracts keep API responses clean and separate from EF entities.
- **Manual mapping** — no AutoMapper; straightforward and easy to debug.
- **SQLite** — zero config, single-file database. Seed data included for demo.
