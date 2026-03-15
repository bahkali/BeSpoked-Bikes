import { Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Dashboard } from './pages/Dashboard';
import { ProductsPage } from './pages/ProductsPage';
import { SalespersonsPage } from './pages/SalespersonsPage';
import { CustomersPage } from './pages/CustomersPage';
import { SalesPage } from './pages/SalesPage';
import { CreateSalePage } from './pages/CreateSalePage';
import { CommissionReportPage } from './pages/CommissionReportPage';

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Dashboard />} />
        <Route path="products" element={<ProductsPage />} />
        <Route path="salespersons" element={<SalespersonsPage />} />
        <Route path="customers" element={<CustomersPage />} />
        <Route path="sales" element={<SalesPage />} />
        <Route path="sales/new" element={<CreateSalePage />} />
        <Route path="report" element={<CommissionReportPage />} />
      </Route>
    </Routes>
  );
}

export default App;
