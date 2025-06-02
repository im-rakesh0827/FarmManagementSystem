import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '@environments/environment';


// Interfaces
export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  role: string;
  fullName: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
     // private apiUrl = 'http://localhost:5254/api/auth'; 
     private apiUrl = `${environment.apiBaseUrl}/api/auth`;
     
     constructor(private http: HttpClient) {}

  /**
   * Register a new user
   */
  register(request: RegisterRequest): Observable<any> {
    console.log('🔧 AuthService: Register request:', request);
    return this.http.post<any>(`${this.apiUrl}/register`, request).pipe(
      tap(() => console.log('✅ Registered successfully')),
      catchError(this.handleError)
    );
  }

  /**
   * Login an existing user
   */
  login(request: LoginRequest): Observable<AuthResponse> {
    console.log('🔧 AuthService: Login request:', request);
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap(response => console.log('✅ Logged in as:', response.email)),
      catchError(this.handleError)
    );
  }

  /**
   * Handle HTTP Errors
   */
  private handleError(error: HttpErrorResponse) {
    console.error('❌ AuthService Error:', error);
    return throwError(() =>
      error.error?.message || 'An unknown error occurred. Please try again.'
    );
  }
}
