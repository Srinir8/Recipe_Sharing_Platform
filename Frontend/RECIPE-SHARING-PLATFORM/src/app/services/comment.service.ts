import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comment } from '../models/comment.model';
import { environment } from '../../environments/environment';

// This interface mirrors your C# CommentInputModel.
export interface CommentInputModel {
  recipeId: string;
  userId: string;
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = `${environment.apiUrl}/Comment`;

  constructor(private http: HttpClient) {}

  getComments(recipeId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/${recipeId}`);
  }

  addComment(input: CommentInputModel): Observable<Comment> {
    return this.http.post<Comment>(this.apiUrl, input);
  }

  deleteComment(commentId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${commentId}`);
  }
}