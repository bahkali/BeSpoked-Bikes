export interface Product {
  id: number;
  name: string;
  manufacturer: string;
  style: string;
  purchasePrice: number;
  salePrice: number;
  qtyOnHand: number;
  commissionPercentage: number;
}

export interface UpdateProduct {
  name: string;
  manufacturer: string;
  style: string;
  purchasePrice: number;
  salePrice: number;
  qtyOnHand: number;
  commissionPercentage: number;
}

export interface Salesperson {
  id: number;
  firstName: string;
  lastName: string;
  address: string;
  phone: string;
  startDate: string;
  terminationDate: string | null;
  manager: string;
}

export interface UpdateSalesperson {
  firstName: string;
  lastName: string;
  address: string;
  phone: string;
  startDate: string;
  terminationDate: string | null;
  manager: string;
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  address: string;
  phone: string;
  startDate: string;
}

export interface Sale {
  id: number;
  productId: number;
  productName: string;
  salespersonId: number;
  salespersonName: string;
  customerId: number;
  customerName: string;
  salesDate: string;
  price: number;
  salespersonCommission: number;
}

export interface CreateSale {
  productId: number;
  salespersonId: number;
  customerId: number;
  salesDate: string;
}

export interface Discount {
  id: number;
  productId: number;
  productName: string;
  beginDate: string;
  endDate: string;
  discountPercentage: number;
}

export interface CommissionReport {
  salespersonId: number;
  salespersonName: string;
  totalSalesCount: number;
  totalSalesAmount: number;
  totalCommission: number;
}

export interface QuarterlyReport {
  year: number;
  quarter: number;
  salespersons: CommissionReport[];
}
