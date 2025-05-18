import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Like } from '../models/like.model';
import { environment } from '../../environments/environment';

// This interface mirrors your C# LikeInputModel.
export interface LikeInputModel {
  recipeId: string;
  userId: string;
}

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  private apiUrl = `${environment.apiUrl}/Like`;

  constructor(private http: HttpClient) {}

  getLikes(recipeId: string): Observable<Like[]> {
    return this.http.get<Like[]>(`${this.apiUrl}/${recipeId}`);
  }

  addLike(input: LikeInputModel): Observable<Like> {
    return this.http.post<Like>(this.apiUrl, input);
  }

  // The API expects the like's ID for deletion.
  deleteLike(likeId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${likeId}`);
  }
}