import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
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
  private baseUrl = `${environment.apiBaseUrl}/auth`;
  private authStatus = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient) {}

  /**
   * Register a new user
   */
  register(request: RegisterRequest): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, request).pipe(
      tap(() => console.log('✅ Registered successfully')),
      catchError(this.handleError)
    );
  }

  /**
   * Login user and store token + user info
   */
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, request).pipe(
      tap(response => {
        console.log('✅ Logged in as:', response.email);
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('auth_user', JSON.stringify(response));
        this.authStatus.next(true);
      }),
      catchError(this.handleError)
    );
  }

  /**
   * Logout user
   */
  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
    this.authStatus.next(false);
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return this.hasToken();
  }

  /**
   * Observable for nav bar etc.
   */
  getAuthStatus(): Observable<boolean> {
    return this.authStatus.asObservable();
  }

  /**
   * Get current user info
   */
  getUser(): AuthResponse | null {
    const user = localStorage.getItem('auth_user');
    return user ? JSON.parse(user) : null;
  }

  /**
   * Get user role
   */
  getRole(): string | null {
    return this.getUser()?.role ?? null;
  }

  /**
   * Internal token check
   */
  private hasToken(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  /**
   * Error handler
   */
  private handleError(error: HttpErrorResponse) {
    console.error('❌ AuthService Error:', error);
    return throwError(() => error.error?.message || 'An unknown error occurred.');
  }


  requestOtp(email: string) {
    return this.http.post(`${this.baseUrl}/request-otp`, { email });
  }

verifyOtp(email: string, otp: string):Observable<any>  {
  return this.http.post(`${this.baseUrl}/verify-otp`, { email, otp });
}

resetPasswordViaOtp(email: string, newPassword: string):Observable<any>  {
  return this.http.post(`${this.baseUrl}/reset-password-otp`, { email, newPassword });
}

}
