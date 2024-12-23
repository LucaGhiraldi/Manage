import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest, LoginResponse } from '../../../M-Models/Login';
import { catchError, map, Observable, throwError } from 'rxjs';
import { RegisterRequest, RegisterResponse } from '../../../M-Models/Register';
import { UserInfo } from '../../../M-Models/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:44390';

  constructor(private httpClient: HttpClient) { }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(map(response => {
        localStorage.setItem('accessToken', response.accessToken);
        document.cookie = `refreshToken=${response.refreshToken};`;
        return response;
    }))
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.httpClient.post<RegisterResponse>(`${this.apiUrl}/register`, request)
      .pipe(
        map(response => {
          // Gestisci la risposta di successo
          console.log('Registration successful:', response);
          return response;
        }),
        catchError(error => {
          // Gestisci la risposta di errore
          console.error('Registration error:', error);
          return throwError(() => error);
        })
      );
  }

  info() : Observable<UserInfo> {
    const token = localStorage.getItem('accessToken');  // Ottieni il token dal localStorage
    if (token) {
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.get<UserInfo>(`${this.apiUrl}/manage/info`, { headers });
    }
    return new Observable(observer => {
      observer.error('Token JWT non trovato');
    });
  }

  resfreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshTokenFromCookie();

    return this.httpClient.post<LoginResponse>(`${this.apiUrl}/refresh`, { refreshToken } )
      .pipe(map(response => {
        localStorage.setItem('accessToken', response.accessToken);
        document.cookie = `refreshToken=${response.refreshToken};`;
        return response;
      }))
  }

  private getRefreshTokenFromCookie(): string | null {
    const cookieString = document.cookie;
    const cookieArray = cookieString.split('; ')

    for (const cookie of cookieArray) {
      const [name, value] = cookie.split('=');

      if (name == 'refreshToken') {
        return value;
      }
    }

    return null;
  }

  logout() {
    localStorage.removeItem('accessToken');
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('accessToken') !== null;
  }

}
