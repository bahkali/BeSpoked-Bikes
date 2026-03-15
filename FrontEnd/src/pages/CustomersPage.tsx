import { useFetch } from '../hooks/useFetch';
import { getCustomers } from '../services/api';
import type { Customer } from '../types';
import { DataTable } from '../components/DataTable';

export function CustomersPage() {
  const { data: customers, loading, error } = useFetch(() => getCustomers());

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  if (loading) {
    return <div className="loading">Loading customers...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div>
      <h2>Customers</h2>

      <DataTable<Customer>
        columns={[
          { header: 'Last Name', accessor: 'lastName' },
          { header: 'First Name', accessor: 'firstName' },
          { header: 'Address', accessor: 'address' },
          { header: 'Phone', accessor: 'phone' },
          { header: 'Start Date', accessor: (row) => formatDate(row.startDate) },
        ]}
        data={[...(customers || [])].sort((a, b) => a.lastName.localeCompare(b.lastName))}
      />
    </div>
  );
}
