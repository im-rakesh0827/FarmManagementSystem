export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role?: string;
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
