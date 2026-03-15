# BeSpoked Bikes — Sales Tracking Application

A full-stack sales tracking application for BeSpoked Bikes, a high-end bicycle shop. The app tracks salesperson commissions, manages product inventory, records sales transactions, and generates quarterly bonus reports.

## Tech Stack

| Layer    | Technology                            |
| -------- | -----------------------------------   |
| Backend  | .NET 8 Web API, Entity Framework Core |
| Database | SQLite                                |
| Frontend | React, TypeScript                     |

## Features

- **Salesperson Management** — View and update salespersons (name, address, phone, start/termination date, manager)
- **Product Management** — View and update products (name, manufacturer, style, pricing, quantity, commission percentage)
- **Customer Directory** — View customer list
- **Sales Recording** — Create new sales and browse sales history with optional date-range filtering
- **Commission Tracking** — Automatically calculates salesperson commission per sale, factoring in active discounts
- **Quarterly Report** — Displays a per-salesperson commission summary for any given quarter

## Data Model

| Entity       | Key Fields                                                                                   |
| ------------ | -------------------------------------------------------------------------------------------- |
| Product      | Name, Manufacturer, Style, Purchase Price, Sale Price, Qty On Hand, Commission Percentage    |
| Salesperson  | First Name, Last Name, Address, Phone, Start Date, Termination Date, Manager                 |
| Customer     | First Name, Last Name, Address, Phone, Start Date                                            |
| Sale         | Product, Salesperson, Customer, Sales Date                                                   |
| Discount     | Product, Begin Date, End Date, Discount Percentage                                           |

## Business Rules

- No duplicate products may be entered.
- No duplicate salespersons may be entered.
- Sale price is adjusted by any active discount on the product at the time of sale.
- Salesperson commission is calculated as: `(Sale Price - Discount) × Commission Percentage`.
- The database is seeded with sample data for testing.

### Backend

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend
```bash
cd frontend
npm install
npm run dev
```

## Project Structure

BeSpoked/
├── backend/          # .NET 8 Web API
│   ├── Controllers/  # API endpoints
│   ├── Models/       # Entity classes
│   ├── Data/         # DbContext, seed data, migrations
│   └── Services/     # Business logic
├── frontend/         # React + TypeScript
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── hooks/
│   │   └── services/  # API client
│   └── package.json
└── README.md

## API Endpoints
Method	Route	Description
GET	/api/salespersons	List all salespersons
PUT	/api/salespersons/{id}	Update a salesperson
GET	/api/products	List all products
PUT	/api/products/{id}	Update a product
GET	/api/customers	List all customers
GET	/api/sales	List sales (optional date filter)
POST	/api/sales	Create a new sale
GET	/api/reports/quarterly	Quarterly commission report