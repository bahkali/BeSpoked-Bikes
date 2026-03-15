import { useState, useCallback } from 'react';
import { useFetch } from '../hooks/useFetch';
import { getProducts, updateProduct } from '../services/api';
import type { Product, UpdateProduct } from '../types';
import { DataTable } from '../components/DataTable';

const STYLES = ['Mountain', 'Road', 'Hybrid', 'Fat Bike', 'BMX', 'Electric'];

export function ProductsPage() {
  const { data: products, loading, error, refetch } = useFetch(() => getProducts());
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [formData, setFormData] = useState<UpdateProduct | null>(null);
  const [saveError, setSaveError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  const handleEdit = (product: Product) => {
    setEditingProduct(product);
    setFormData({
      name: product.name,
      manufacturer: product.manufacturer,
      style: product.style,
      purchasePrice: product.purchasePrice,
      salePrice: product.salePrice,
      qtyOnHand: product.qtyOnHand,
      commissionPercentage: product.commissionPercentage,
    });
    setSaveError(null);
  };

  const handleClose = () => {
    setEditingProduct(null);
    setFormData(null);
    setSaveError(null);
  };

  const handleSave = useCallback(async () => {
    if (!editingProduct || !formData) return;

    try {
      setSaving(true);
      setSaveError(null);
      await updateProduct(editingProduct.id, formData);
      handleClose();
      refetch();
    } catch (err) {
      setSaveError(err instanceof Error ? err.message : 'Failed to update product');
    } finally {
      setSaving(false);
    }
  }, [editingProduct, formData, refetch]);

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  if (loading) {
    return <div className="loading">Loading products...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div>
      <h2>Products</h2>

      <DataTable<Product>
        columns={[
          { header: 'Name', accessor: 'name' },
          { header: 'Manufacturer', accessor: 'manufacturer' },
          { header: 'Style', accessor: 'style' },
          { header: 'Purchase Price', accessor: (row) => formatCurrency(row.purchasePrice) },
          { header: 'Sale Price', accessor: (row) => formatCurrency(row.salePrice) },
          { header: 'Qty On Hand', accessor: 'qtyOnHand' },
          {
            header: 'Commission %',
            accessor: (row) => `${row.commissionPercentage}%`,
          },
          {
            header: 'Edit',
            accessor: (row) => (
              <button className="btn btn-sm btn-primary" onClick={() => handleEdit(row)}>
                Edit
              </button>
            ),
          },
        ]}
        data={products || []}
      />

      {editingProduct && formData && (
        <div className="modal-overlay" onClick={handleClose}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h3>Edit Product</h3>
            {saveError && <div className="error">{saveError}</div>}
            <div className="form-grid">
              <div className="form-group full-width">
                <label>Name</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                />
              </div>
              <div className="form-group full-width">
                <label>Manufacturer</label>
                <input
                  type="text"
                  value={formData.manufacturer}
                  onChange={(e) => setFormData({ ...formData, manufacturer: e.target.value })}
                />
              </div>
              <div className="form-group full-width">
                <label>Style</label>
                <select
                  value={formData.style}
                  onChange={(e) => setFormData({ ...formData, style: e.target.value })}
                >
                  {STYLES.map((style) => (
                    <option key={style} value={style}>
                      {style}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Purchase Price</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.purchasePrice}
                  onChange={(e) =>
                    setFormData({ ...formData, purchasePrice: parseFloat(e.target.value) })
                  }
                />
              </div>
              <div className="form-group">
                <label>Sale Price</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.salePrice}
                  onChange={(e) =>
                    setFormData({ ...formData, salePrice: parseFloat(e.target.value) })
                  }
                />
              </div>
              <div className="form-group">
                <label>Qty On Hand</label>
                <input
                  type="number"
                  value={formData.qtyOnHand}
                  onChange={(e) =>
                    setFormData({ ...formData, qtyOnHand: parseInt(e.target.value) })
                  }
                />
              </div>
              <div className="form-group">
                <label>Commission %</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.commissionPercentage}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      commissionPercentage: parseFloat(e.target.value),
                    })
                  }
                />
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
