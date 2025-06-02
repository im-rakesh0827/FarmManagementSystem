import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
     // private apiUrl = 'http://localhost:5254/api/auth';
     private apiUrl = `${environment.apiBaseUrl}/api/auth`;

  constructor(private http: HttpClient) {}
     getAllUsers(): Observable<User[]> {
          // console.log(this.apiUrl1);
    return this.http.get<User[]>(`${this.apiUrl}/allUsers`);
     }
     
     deleteUser(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/users/${id}`);
}

}
