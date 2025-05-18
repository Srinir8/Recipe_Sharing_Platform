import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { shareReplay } from 'rxjs/operators';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/User`;
  private currentUser$?: Observable<User>;

  constructor(private http: HttpClient) {}

  getCurrentUser(): Observable<User> {
    if (!this.currentUser$) {
      this.currentUser$ = this.http.get<User>(`${this.apiUrl}/me`).pipe(
        shareReplay(1)
      );
    }
    return this.currentUser$;
  }

  // Clear cache for current user (e.g. upon logout)
  clearCache(): void {
    this.currentUser$ = undefined;
  }

  addUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }
}