import { useState, useCallback } from 'react';
import { useFetch } from '../hooks/useFetch';
import { getSalespersons, updateSalesperson } from '../services/api';
import type { Salesperson, UpdateSalesperson } from '../types';
import { DataTable } from '../components/DataTable';

export function SalespersonsPage() {
  const { data: salespersons, loading, error, refetch } = useFetch(() => getSalespersons());
  const [editingSalesperson, setEditingSalesperson] = useState<Salesperson | null>(null);
  const [formData, setFormData] = useState<UpdateSalesperson | null>(null);
  const [saveError, setSaveError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  const handleEdit = (salesperson: Salesperson) => {
    setEditingSalesperson(salesperson);
    setFormData({
      firstName: salesperson.firstName,
      lastName: salesperson.lastName,
      address: salesperson.address,
      phone: salesperson.phone,
      startDate: salesperson.startDate.split('T')[0],
      terminationDate: salesperson.terminationDate ? salesperson.terminationDate.split('T')[0] : '',
      manager: salesperson.manager,
    });
    setSaveError(null);
  };

  const handleClose = () => {
    setEditingSalesperson(null);
    setFormData(null);
    setSaveError(null);
  };

  const handleSave = useCallback(async () => {
    if (!editingSalesperson || !formData) return;

    try {
      setSaving(true);
      setSaveError(null);
      const updateData: UpdateSalesperson = {
        ...formData,
        startDate: formData.startDate,
        terminationDate: formData.terminationDate || null,
      };
      await updateSalesperson(editingSalesperson.id, updateData);
      handleClose();
      refetch();
    } catch (err) {
      setSaveError(err instanceof Error ? err.message : 'Failed to update salesperson');
    } finally {
      setSaving(false);
    }
  }, [editingSalesperson, formData, refetch]);

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  if (loading) {
    return <div className="loading">Loading salespersons...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div>
      <h2>Salespersons</h2>

      <DataTable<Salesperson>
        columns={[
          { header: 'Last Name', accessor: 'lastName' },
          { header: 'First Name', accessor: 'firstName' },
          { header: 'Address', accessor: 'address' },
          { header: 'Phone', accessor: 'phone' },
          { header: 'Start Date', accessor: (row) => formatDate(row.startDate) },
          {
            header: 'Status',
            accessor: (row) => {
              if (row.terminationDate) {
                return (
                  <span className="badge badge-inactive">
                    Terminated {formatDate(row.terminationDate)}
                  </span>
                );
              }
              return <span className="badge badge-active">Active</span>;
            },
          },
          { header: 'Manager', accessor: 'manager' },
          {
            header: 'Edit',
            accessor: (row) => (
              <button className="btn btn-sm btn-primary" onClick={() => handleEdit(row)}>
                Edit
              </button>
            ),
          },
        ]}
        data={[...(salespersons || [])].sort((a, b) => a.lastName.localeCompare(b.lastName))}
      />

      {editingSalesperson && formData && (
        <div className="modal-overlay" onClick={handleClose}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h3>Edit Salesperson</h3>
            {saveError && <div className="error">{saveError}</div>}
            <div className="form-grid">
              <div className="form-group">
                <label>First Name</label>
                <input
                  type="text"
                  value={formData.firstName}
                  onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Last Name</label>
                <input
                  type="text"
                  value={formData.lastName}
                  onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                />
              </div>
              <div className="form-group full-width">
                <label>Address</label>
                <input
                  type="text"
                  value={formData.address}
                  onChange={(e) => setFormData({ ...formData, address: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Phone</label>
                <input
                  type="text"
                  value={formData.phone}
                  onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Manager</label>
                <input
                  type="text"
                  value={formData.manager}
                  onChange={(e) => setFormData({ ...formData, manager: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Start Date</label>
                <input
                  type="date"
                  value={formData.startDate}
                  onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Termination Date (optional)</label>
                <input
                  type="date"
                  value={formData.terminationDate}
                  onChange={(e) =>
                    setFormData({ ...formData, terminationDate: e.target.value || null })
                  }
                />
                <small style={{ color: '#6c757d', fontSize: '12px' }}>
                  Leave empty to remove termination date
                </small>
              </div>
            </div>
            <div className="modal-actions">
              <button className="btn btn-secondary" onClick={handleClose} disabled={saving}>
                Cancel
              </button>
              <button className="btn btn-primary" onClick={handleSave} disabled={saving}>
                {saving ? 'Saving...' : 'Save'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
