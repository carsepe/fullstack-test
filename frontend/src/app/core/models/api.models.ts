export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface PagedQuery {
  page: number;
  pageSize: number;
  search?: string;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
  includeInactive?: boolean;
}

export interface Provider {
  id: string;
  nit: string;
  name: string;
  website: string;
  email: string;
  isActive: boolean;
  createdAtUtc: string;
  createdBy: string;
  updatedAtUtc: string | null;
  updatedBy: string | null;
}

export interface ProviderService {
  id: string;
  providerId: string;
  name: string;
  hourlyRateUsd: number;
  isActive: boolean;
  createdAtUtc: string;
  createdBy: string;
  updatedAtUtc: string | null;
  updatedBy: string | null;
}

export interface DashboardSummary {
  totalProviders: number;
  totalProviderServices: number;
  averageHourlyRateUsd: number | null;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
}

export interface CreateProviderRequest {
  nit: string;
  name: string;
  website: string;
  email: string;
}

export interface UpdateProviderRequest {
  nit: string;
  name: string;
  website: string;
  email: string;
}

export interface CreateProviderServiceRequest {
  providerId: string;
  name: string;
  hourlyRateUsd: number;
}

export interface UpdateProviderServiceRequest {
  name: string;
  hourlyRateUsd: number;
}
