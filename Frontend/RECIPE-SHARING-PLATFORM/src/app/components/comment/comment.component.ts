import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommentService, CommentInputModel } from '../../services/comment.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Comment } from '../../models/comment.model';

@Component({
  selector: 'app-comment',
  standalone: true,
  imports: [CommonModule, FormsModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() recipeId!: string;
  comments: Comment[] = [];
  newComment: string = '';

  constructor(private commentService: CommentService) {}

  ngOnInit(): void {
    this.fetchComments();
  }

  fetchComments(): void {
    this.commentService.getComments(this.recipeId).subscribe({
      next: (data: Comment[]) => (this.comments = data),
      error: (err) => console.error('Failed to fetch comments:', err)
    });
  }

  addComment(): void {
    const token = localStorage.getItem('token');
    if (!token) {
      alert('Please login to add a comment.');
      return;
    }
    if (this.newComment.trim()) {
      const input: CommentInputModel = {
        recipeId: this.recipeId,
        userId: localStorage.getItem('userId') || 'anonymous',
        content: this.newComment
      };
      this.commentService.addComment(input).subscribe({
        next: (createdComment: Comment) => {
          this.comments.push(createdComment);
          this.newComment = '';
        },
        error: (err) => console.error('Failed to add comment:', err)
      });
    }
  }

  deleteComment(commentId: string, index: number): void {
    const token = localStorage.getItem('token');
    if (!token) {
      alert('Please login to delete a comment.');
      return;
    }
    this.commentService.deleteComment(commentId).subscribe({
      next: () => this.comments.splice(index, 1),
      error: (err) => console.error('Failed to delete comment:', err)
    });
  }
}