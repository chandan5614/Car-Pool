import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.dev';
import { User } from '../models/user.model';
import { LoginModel } from '../models/login.model';
import { RegisterModel } from '../models/register.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = `${environment.apiBaseUrl}/api/User`;
  private authUrl = `${environment.apiBaseUrl}/api/Auth`;

  constructor(private http: HttpClient) {}

  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  registerUser(user: RegisterModel): Observable<any> {
    return this.http.post<any>(this.apiUrl, user);
  }

  loginUser(user: LoginModel): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/login`, user);
  }
}
