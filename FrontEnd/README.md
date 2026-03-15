# BeSpoked Bikes — Frontend

React + TypeScript SPA for the BeSpoked Bikes sales tracking system. Communicates with the .NET backend API via a Vite dev proxy.

## Tech Stack

- **React 19** with TypeScript
- **React Router v7** for client-side routing
- **Vite 8** for dev server and bundling

## Getting Started

```bash
npm install
npm run dev
```

The dev server starts on **http://localhost:3000** and proxies `/api` requests to the backend at `http://localhost:5194`.

Make sure the .NET backend is running first.

## Project Structure

```
src/
├── components/       # Reusable UI components
│   ├── Layout.tsx    # Sidebar navigation + page outlet
│   └── DataTable.tsx # Generic table component
├── pages/
│   ├── Dashboard.tsx           # Stats overview + recent sales
│   ├── ProductsPage.tsx        # Product list with inline edit modal
│   ├── SalespersonsPage.tsx    # Salesperson list with inline edit modal
│   ├── CustomersPage.tsx       # Customer list (read-only)
│   ├── SalesPage.tsx           # Sales history with date filters
│   ├── CreateSalePage.tsx      # New sale form with order preview
│   └── CommissionReportPage.tsx # Quarterly commission report
├── hooks/
│   └── useFetch.ts   # Generic data-fetching hook (loading/error/refetch)
├── services/
│   └── api.ts        # API client — all backend calls in one place
├── types/
│   └── index.ts      # Shared TypeScript interfaces
├── App.tsx           # Route definitions
├── main.tsx          # Entry point
└── index.css         # Global styles
```

## Pages & Routes

| Route          | Page                   | Description                              |
| -------------- | ---------------------- | ---------------------------------------- |
| `/`            | Dashboard              | Summary stats and recent sales           |
| `/products`    | ProductsPage           | View/edit products                       |
| `/salespersons`| SalespersonsPage       | View/edit salespersons                   |
| `/customers`   | CustomersPage          | View customers                           |
| `/sales`       | SalesPage              | Sales history with date-range filtering  |
| `/sales/new`   | CreateSalePage         | Record a new sale                        |
| `/report`      | CommissionReportPage   | Quarterly commission breakdown           |

## Scripts

| Command         | Description                  |
| --------------- | ---------------------------- |
| `npm run dev`   | Start dev server on port 3000|
| `npm run build` | Type-check and build for prod|
| `npm run lint`  | Run ESLint                   |
| `npm run preview`| Preview production build    |
