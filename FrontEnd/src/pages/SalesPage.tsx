import { useState, useMemo } from 'react';
import { getSales } from '../services/api';
import type { Sale } from '../types';
import { DataTable } from '../components/DataTable';
import { useFetch } from '../hooks/useFetch';

export function SalesPage() {
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  const { data: sales, loading, error } = useFetch(
    () => getSales(startDate || undefined, endDate || undefined),
    [startDate, endDate]
  );

  const currentSales = sales ?? [];

  const totals = useMemo(() => ({
    totalRevenue: currentSales.reduce((sum, sale) => sum + sale.price, 0),
    totalCommission: currentSales.reduce((sum, sale) => sum + sale.salespersonCommission, 0),
  }), [currentSales]);

  const formatCurrency = (amount: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount);

  const formatDate = (dateString: string) =>
    new Date(dateString).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });

  const handleClearFilters = () => {
    setStartDate('');
    setEndDate('');
  };

  if (loading && currentSales.length === 0) {
    return <div className="loading">Loading sales...</div>;
  }

  return (
    <div>
      <h2>Sales History</h2>

      <div className="filter-bar">
        <div className="form-group" style={{ marginBottom: 0 }}>
          <label>Start Date</label>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </div>
        <div className="form-group" style={{ marginBottom: 0 }}>
          <label>End Date</label>
          <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
        </div>
        <button className="btn btn-secondary" onClick={handleClearFilters}>
          Clear
        </button>
      </div>

      <div className="summary-bar">
        <strong>{currentSales.length} sales</strong>
        <span>Revenue: {formatCurrency(totals.totalRevenue)}</span>
        <span>Commission: {formatCurrency(totals.totalCommission)}</span>
      </div>

      {error && <div className="error">{error}</div>}

      <DataTable<Sale>
        columns={[
          { header: 'Date', accessor: (row) => formatDate(row.salesDate) },
          { header: 'Product', accessor: 'productName' },
          { header: 'Customer', accessor: 'customerName' },
          { header: 'Salesperson', accessor: 'salespersonName' },
          { header: 'Price', accessor: (row) => formatCurrency(row.price) },
          { header: 'Commission', accessor: (row) => formatCurrency(row.salespersonCommission) },
        ]}
        data={currentSales}
      />
    </div>
  );
}
