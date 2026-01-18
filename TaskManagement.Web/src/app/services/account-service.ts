import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BehaviorSubject, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { RegisterDto } from '../dtos/RegisterDto';
import { LoginResponseDto } from '../dtos/LoginResponseDto';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  
  private url = `${environment.apiUrl}/Accounts`;
  private loggedIn = new BehaviorSubject<boolean>(this.isLoggedIn());
  loginStatusChange = this.loggedIn.asObservable();
  
  constructor(private http: HttpClient) { }

  login(email: string, password: string) {
    return this.http.post<LoginResponseDto>(`${this.url}/login`, { email, password })
          .pipe(
            tap(response => {
              if (response && response.token) {
                localStorage.setItem('user', JSON.stringify(response));
                this.loggedIn.next(true);
              }
            })
          );
  }

  logout() {
    localStorage.removeItem('user');
    this.loggedIn.next(false);
  }

  register(registerDto: RegisterDto) {
    return this.http.post(`${this.url}/register`, registerDto);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('user');
  }

  getToken(): string | null {
    return this.getCurrentUser()?.token ?? null;
  }

  getCurrentUser(): LoginResponseDto | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}
