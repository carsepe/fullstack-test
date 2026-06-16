import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../constants/api.constants';
import {
  CreateProviderRequest,
  CreateProviderServiceRequest,
  DashboardSummary,
  PagedQuery,
  PagedResult,
  Provider,
  ProviderService,
  UpdateProviderRequest,
  UpdateProviderServiceRequest,
} from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private readonly http: HttpClient) {}

  getDashboardSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${API_URL}/dashboard/summary`);
  }

  getProviders(query: PagedQuery): Observable<PagedResult<Provider>> {
    return this.http.get<PagedResult<Provider>>(`${API_URL}/providers`, {
      params: this.toParams(query),
    });
  }

  createProvider(request: CreateProviderRequest): Observable<Provider> {
    return this.http.post<Provider>(`${API_URL}/providers`, request);
  }

  updateProvider(id: string, request: UpdateProviderRequest): Observable<Provider> {
    return this.http.put<Provider>(`${API_URL}/providers/${id}`, request);
  }

  activateProvider(id: string): Observable<void> {
    return this.http.patch<void>(`${API_URL}/providers/${id}/activate`, null);
  }

  deactivateProvider(id: string): Observable<void> {
    return this.http.patch<void>(`${API_URL}/providers/${id}/deactivate`, null);
  }

  getProviderServices(
    query: PagedQuery,
    providerId?: string
  ): Observable<PagedResult<ProviderService>> {
    let params = this.toParams(query);

    if (providerId) {
      params = params.set('providerId', providerId);
    }

    return this.http.get<PagedResult<ProviderService>>(`${API_URL}/provider-services`, {
      params,
    });
  }

  createProviderService(request: CreateProviderServiceRequest): Observable<ProviderService> {
    return this.http.post<ProviderService>(`${API_URL}/provider-services`, request);
  }

  updateProviderService(
    id: string,
    request: UpdateProviderServiceRequest
  ): Observable<ProviderService> {
    return this.http.put<ProviderService>(`${API_URL}/provider-services/${id}`, request);
  }

  activateProviderService(id: string): Observable<void> {
    return this.http.patch<void>(`${API_URL}/provider-services/${id}/activate`, null);
  }

  deactivateProviderService(id: string): Observable<void> {
    return this.http.patch<void>(`${API_URL}/provider-services/${id}/deactivate`, null);
  }

  private toParams(query: PagedQuery): HttpParams {
    let params = new HttpParams()
      .set('page', query.page)
      .set('pageSize', query.pageSize)
      .set('sortDirection', query.sortDirection ?? 'asc');

    if (query.search) {
      params = params.set('search', query.search);
    }

    if (query.sortBy) {
      params = params.set('sortBy', query.sortBy);
    }

    if (query.includeInactive) {
      params = params.set('includeInactive', 'true');
    }

    return params;
  }
}
