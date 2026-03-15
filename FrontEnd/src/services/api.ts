import type {
  Product,
  UpdateProduct,
  Salesperson,
  UpdateSalesperson,
  Customer,
  Sale,
  CreateSale,
  Discount,
  QuarterlyReport,
} from '../types';

async function api<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`/api${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });
  if (!res.ok) {
    const body = await res.text();
    const isStackTrace = body.includes('   at ') || body.includes('Exception:');
    throw new Error(isStackTrace ? `Server error (${res.status})` : body || `Request failed (${res.status})`);
  }
  return res.json();
}

export async function getProducts(): Promise<Product[]> {
  return api('/products');
}

export async function getProduct(id: number): Promise<Product> {
  return api(`/products/${id}`);
}

export async function updateProduct(id: number, data: UpdateProduct): Promise<void> {
  await api(`/products/${id}`, { method: 'PUT', body: JSON.stringify(data) });
}

export async function getSalespersons(): Promise<Salesperson[]> {
  return api('/salespersons');
}

export async function getSalesperson(id: number): Promise<Salesperson> {
  return api(`/salespersons/${id}`);
}

export async function updateSalesperson(id: number, data: UpdateSalesperson): Promise<void> {
  await api(`/salespersons/${id}`, { method: 'PUT', body: JSON.stringify(data) });
}

export async function getCustomers(): Promise<Customer[]> {
  return api('/customers');
}

export async function getSales(startDate?: string, endDate?: string): Promise<Sale[]> {
  const params = new URLSearchParams();
  if (startDate) params.set('startDate', startDate);
  if (endDate) params.set('endDate', endDate);
  const query = params.toString();
  return api(`/sales${query ? `?${query}` : ''}`);
}

export async function createSale(data: CreateSale): Promise<Sale> {
  return api('/sales', { method: 'POST', body: JSON.stringify(data) });
}

export async function getDiscounts(): Promise<Discount[]> {
  return api('/discounts');
}

export async function getQuarterlyReport(year: number, quarter: number): Promise<QuarterlyReport> {
  return api(`/sales/quarterly-report?year=${year}&quarter=${quarter}`);
}
