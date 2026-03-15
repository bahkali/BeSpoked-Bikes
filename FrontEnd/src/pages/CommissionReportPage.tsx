import { useState, useMemo } from 'react';
import { getQuarterlyReport } from '../services/api';
import type { CommissionReport } from '../types';
import { DataTable } from '../components/DataTable';
import { useFetch } from '../hooks/useFetch';

export function CommissionReportPage() {
  const [year, setYear] = useState(new Date().getFullYear());
  const [quarter, setQuarter] = useState(Math.floor((new Date().getMonth() + 3) / 3));

  const { data: report, loading, error } = useFetch(
    () => getQuarterlyReport(year, quarter),
    [year, quarter]
  );

  const grandTotals = useMemo(() => {
    if (!report) return null;
    return report.salespersons.reduce(
      (acc, sp) => ({
        totalSalesCount: acc.totalSalesCount + sp.totalSalesCount,
        totalSalesAmount: acc.totalSalesAmount + sp.totalSalesAmount,
        totalCommission: acc.totalCommission + sp.totalCommission,
      }),
      { totalSalesCount: 0, totalSalesAmount: 0, totalCommission: 0 }
    );
  }, [report]);

  const formatCurrency = (amount: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount);

  const years = [2024, 2025, 2026];

  if (loading) return <div className="loading">Loading report...</div>;

  return (
    <div>
      <h2>Commission Report — Q{quarter} {year}</h2>

      <div className="filter-bar">
        <div className="form-group" style={{ marginBottom: 0 }}>
          <label>Year</label>
          <select value={year} onChange={(e) => setYear(parseInt(e.target.value))}>
            {years.map((y) => (
              <option key={y} value={y}>{y}</option>
            ))}
          </select>
        </div>
        <div className="form-group" style={{ marginBottom: 0 }}>
          <label>Quarter</label>
          <select value={quarter} onChange={(e) => setQuarter(parseInt(e.target.value))}>
            <option value={1}>Q1</option>
            <option value={2}>Q2</option>
            <option value={3}>Q3</option>
            <option value={4}>Q4</option>
          </select>
        </div>
      </div>

      {error && <div className="error">{error}</div>}

      {report && report.salespersons.length > 0 ? (
        <>
          <DataTable<CommissionReport>
            columns={[
              { header: 'Salesperson', accessor: 'salespersonName' },
              { header: '# of Sales', accessor: 'totalSalesCount' },
              { header: 'Total Sales', accessor: (row) => formatCurrency(row.totalSalesAmount) },
              { header: 'Total Commission', accessor: (row) => formatCurrency(row.totalCommission) },
            ]}
            data={report.salespersons}
          />

          {grandTotals && (
            <div
              style={{
                marginTop: '16px',
                padding: '16px',
                background: '#f8f9fa',
                borderRadius: '8px',
                fontWeight: 'bold',
              }}
            >
              <div style={{ display: 'flex', gap: '24px' }}>
                <span>Total Sales: {grandTotals.totalSalesCount}</span>
                <span>Total Revenue: {formatCurrency(grandTotals.totalSalesAmount)}</span>
                <span>Total Commission: {formatCurrency(grandTotals.totalCommission)}</span>
              </div>
            </div>
          )}
        </>
      ) : (
        <div className="loading">No sales recorded for this quarter.</div>
      )}
    </div>
  );
}
