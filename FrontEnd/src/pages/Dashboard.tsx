import { useMemo } from 'react';
import { getProducts, getSalespersons, getSales } from '../services/api';
import type { Product, Salesperson, Sale } from '../types';
import { DataTable } from '../components/DataTable';
import { useFetch } from '../hooks/useFetch';

export function Dashboard() {
  
  const { data, loading, error } = useFetch(() =>
    Promise.all([getProducts(), getSalespersons(), getSales()])
  );
  const [products, salespersons, sales] = data ?? [[] as Product[], [] as Salesperson[], [] as Sale[]];

  const stats = useMemo(() => ({
    totalSales: sales.length,
    totalRevenue: sales.reduce((sum, sale) => sum + sale.price, 0),
    totalCommission: sales.reduce((sum, sale) => sum + sale.salespersonCommission, 0),
    productsInStock: products.reduce((sum, p) => sum + p.qtyOnHand, 0),
    activeSalespersons: salespersons.filter((sp) => sp.terminationDate === null).length,
  }), [products, salespersons, sales]);

  const recentSales = useMemo(() => {
    return [...sales]
      .sort((a, b) => new Date(b.salesDate).getTime() - new Date(a.salesDate).getTime())
      .slice(0, 5);
  }, [sales]);

  const formatCurrency = (amount: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount);

  const formatDate = (dateString: string) =>
    new Date(dateString).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });

  if (loading) return <div className="loading">Loading dashboard...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div>
      <h2>Dashboard</h2>

      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-value">{stats.totalSales}</div>
          <div className="stat-label">Total Sales</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{formatCurrency(stats.totalRevenue)}</div>
          <div className="stat-label">Total Revenue</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{formatCurrency(stats.totalCommission)}</div>
          <div className="stat-label">Total Commission</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{stats.productsInStock}</div>
          <div className="stat-label">Products In Stock</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{stats.activeSalespersons}</div>
          <div className="stat-label">Active Salespersons</div>
        </div>
      </div>

      <h3 style={{ marginTop: '32px', marginBottom: '16px' }}>Recent Sales</h3>
      <DataTable<Sale>
        columns={[
          { header: 'Date', accessor: (row) => formatDate(row.salesDate) },
          { header: 'Product', accessor: 'productName' },
          { header: 'Customer', accessor: 'customerName' },
          { header: 'Salesperson', accessor: 'salespersonName' },
          { header: 'Price', accessor: (row) => formatCurrency(row.price) },
          { header: 'Commission', accessor: (row) => formatCurrency(row.salespersonCommission) },
        ]}
        data={recentSales}
        emptyMessage="No sales recorded yet."
      />
    </div>
  );
}
