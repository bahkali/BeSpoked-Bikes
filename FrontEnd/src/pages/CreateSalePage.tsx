import { useState, useMemo } from 'react';
import { createSale, getProducts, getSalespersons, getCustomers } from '../services/api';
import type { Product, Salesperson, Customer, CreateSale } from '../types';
import { useNavigate } from 'react-router-dom';
import { useFetch } from '../hooks/useFetch';

export function CreateSalePage() {
  const navigate = useNavigate();

  const { data: dropdownData, loading, error: loadError } = useFetch(() =>
    Promise.all([getProducts(), getSalespersons(), getCustomers()])
  );
  const [products, allSalespersons, customers] = dropdownData ?? [[] as Product[], [] as Salesperson[], [] as Customer[]];
  const salespersons = useMemo(() => allSalespersons.filter((sp) => sp.terminationDate === null), [allSalespersons]);

  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState<{ sale: any } | null>(null);

  const [formData, setFormData] = useState<CreateSale>({
    productId: 0,
    salespersonId: 0,
    customerId: 0,
    salesDate: new Date().toISOString().split('T')[0],
  });

  const selectedProduct = products.find((p) => p.id === formData.productId);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.productId || !formData.salespersonId || !formData.customerId) {
      setError('Please fill in all fields');
      return;
    }

    try {
      setSubmitting(true);
      setError(null);
      const sale = await createSale({
        ...formData,
        salesDate: new Date(formData.salesDate).toISOString(),
      });
      setSuccess({ sale });
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create sale');
    } finally {
      setSubmitting(false);
    }
  };

  const formatCurrency = (amount: number) =>
    new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(amount);

  if (loading) return <div className="loading">Loading...</div>;
  if (loadError) return <div className="error">{loadError}</div>;

  if (success) {
    const { sale } = success;
    return (
      <div>
        <h2>Sale Created Successfully</h2>
        <div className="success">
          <p><strong>Product:</strong> {sale.productName}</p>
          <p><strong>Price:</strong> {formatCurrency(sale.price)}</p>
          <p><strong>Commission Earned:</strong> {formatCurrency(sale.salespersonCommission)}</p>
        </div>
        <div style={{ marginTop: '24px', display: 'flex', gap: '12px' }}>
          <button
            className="btn btn-primary"
            onClick={() => {
              setSuccess(null);
              setFormData({
                productId: 0,
                salespersonId: 0,
                customerId: 0,
                salesDate: new Date().toISOString().split('T')[0],
              });
            }}
          >
            Create Another
          </button>
          <button className="btn btn-secondary" onClick={() => navigate('/sales')}>
            View Sales History
          </button>
        </div>
      </div>
    );
  }

  return (
    <div>
      <h2>Create New Sale</h2>
      <p className="page-subtitle">Fill in the details below to record a new sale.</p>

      {error && <div className="error">{error}</div>}

      <div className="sale-form-layout">
        <form onSubmit={handleSubmit} className="sale-form-card">
          <div className="sale-form-section">
            <div className="sale-form-section-title">Sale Details</div>
            <div className="form-grid">
              <div className="form-group full-width">
                <label>Product</label>
                <select
                  value={formData.productId}
                  onChange={(e) =>
                    setFormData({ ...formData, productId: parseInt(e.target.value) })
                  }
                  required
                >
                  <option value={0}>Select a product</option>
                  {products.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.name} - {formatCurrency(p.salePrice)}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Salesperson</label>
                <select
                  value={formData.salespersonId}
                  onChange={(e) =>
                    setFormData({ ...formData, salespersonId: parseInt(e.target.value) })
                  }
                  required
                >
                  <option value={0}>Select a salesperson</option>
                  {salespersons.map((sp) => (
                    <option key={sp.id} value={sp.id}>
                      {sp.firstName} {sp.lastName}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Customer</label>
                <select
                  value={formData.customerId}
                  onChange={(e) =>
                    setFormData({ ...formData, customerId: parseInt(e.target.value) })
                  }
                  required
                >
                  <option value={0}>Select a customer</option>
                  {customers.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.firstName} {c.lastName}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group full-width">
                <label>Sales Date</label>
                <input
                  type="date"
                  value={formData.salesDate}
                  onChange={(e) => setFormData({ ...formData, salesDate: e.target.value })}
                  required
                />
              </div>
            </div>
          </div>

          {selectedProduct && (
            <div className="sale-form-section">
              <div className="sale-form-section-title">Order Summary</div>
              <div className="sale-preview">
                <div className="sale-preview-row">
                  <span className="sale-preview-label">Product</span>
                  <span className="sale-preview-value">{selectedProduct.name}</span>
                </div>
                <div className="sale-preview-row">
                  <span className="sale-preview-label">Sale Price</span>
                  <span className="sale-preview-value sale-preview-price">
                    {formatCurrency(selectedProduct.salePrice)}
                  </span>
                </div>
                <div className="sale-preview-row">
                  <span className="sale-preview-label">Commission Rate</span>
                  <span className="sale-preview-value">{selectedProduct.commissionPercentage}%</span>
                </div>
                <div className="sale-preview-divider" />
                <div className="sale-preview-row">
                  <span className="sale-preview-label">Estimated Commission</span>
                  <span className="sale-preview-value sale-preview-highlight">
                    {formatCurrency(selectedProduct.salePrice * selectedProduct.commissionPercentage / 100)}
                  </span>
                </div>
              </div>
            </div>
          )}

          <div className="sale-form-actions">
            <button type="button" className="btn btn-secondary" onClick={() => navigate('/sales')}>
              Cancel
            </button>
            <button type="submit" className="btn btn-primary btn-lg" disabled={submitting}>
              {submitting ? 'Creating...' : 'Create Sale'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
