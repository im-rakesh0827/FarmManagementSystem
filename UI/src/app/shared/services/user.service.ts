import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = `${environment.apiBaseUrl}/auth`;
  constructor(private http: HttpClient) {}
     getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/allUsers`);
     }
     
deleteUser(id: number): Observable<void> {
  return this.http.delete<void>(`${this.baseUrl}/users/${id}`);
}
  
//   getProfile(email: string): Observable<User> {
//   return this.http.get<User>(`${this.baseUrl}/profile?email=${email}`);
  // }
  
  getProfile(email: string): Observable<User> {
  return this.http.get<User>(`${this.baseUrl}/user/${email}`);
}



}
