import axios from 'axios';
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

const api = axios.create({
  baseURL: '/api',
});

export async function getProducts(): Promise<Product[]> {
  const response = await api.get<Product[]>('/products');
  return response.data;
}

export async function getProduct(id: number): Promise<Product> {
  const response = await api.get<Product>(`/products/${id}`);
  return response.data;
}

export async function updateProduct(id: number, data: UpdateProduct): Promise<void> {
  await api.put(`/products/${id}`, data);
}

export async function getSalespersons(): Promise<Salesperson[]> {
  const response = await api.get<Salesperson[]>('/salespersons');
  return response.data;
}

export async function getSalesperson(id: number): Promise<Salesperson> {
  const response = await api.get<Salesperson>(`/salespersons/${id}`);
  return response.data;
}

export async function updateSalesperson(id: number, data: UpdateSalesperson): Promise<void> {
  await api.put(`/salespersons/${id}`, data);
}

export async function getCustomers(): Promise<Customer[]> {
  const response = await api.get<Customer[]>('/customers');
  return response.data;
}

export async function getSales(startDate?: string, endDate?: string): Promise<Sale[]> {
  const params: Record<string, string> = {};
  if (startDate) params.startDate = startDate;
  if (endDate) params.endDate = endDate;
  const response = await api.get<Sale[]>('/sales', { params });
  return response.data;
}

export async function createSale(data: CreateSale): Promise<Sale> {
  const response = await api.post<Sale>('/sales', data);
  return response.data;
}

export async function getDiscounts(): Promise<Discount[]> {
  const response = await api.get<Discount[]>('/discounts');
  return response.data;
}

export async function getQuarterlyReport(year: number, quarter: number): Promise<QuarterlyReport> {
  const response = await api.get<QuarterlyReport>('/sales/quarterly-report', {
    params: { year, quarter },
  });
  return response.data;
}
