import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { API_URL, TOKEN_STORAGE_KEY } from '../constants/api.constants';
import { LoginRequest, LoginResponse } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly token = signal<string | null>(this.readStoredToken());

  constructor(private readonly http: HttpClient) {}

  isAuthenticated(): boolean {
    return !!this.token();
  }

  getToken(): string | null {
    return this.token();
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_URL}/auth/login`, request).pipe(
      tap((response) => this.setToken(response.token))
    );
  }

  logout(): void {
    this.setToken(null);
  }

  private setToken(value: string | null): void {
    this.token.set(value);

    if (value) {
      localStorage.setItem(TOKEN_STORAGE_KEY, value);
      return;
    }

    localStorage.removeItem(TOKEN_STORAGE_KEY);
  }

  private readStoredToken(): string | null {
    return localStorage.getItem(TOKEN_STORAGE_KEY);
  }
}
