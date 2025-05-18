import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LikeService, LikeInputModel } from '../../services/like.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Like } from '../../models/like.model';

@Component({
  selector: 'app-like',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './like.component.html',
  styleUrls: ['./like.component.css']
})
export class LikeComponent implements OnInit {
  @Input() recipeId!: string;
  @Input() disabled: boolean = false; // Added disabled input

  likesCount: number = 0;
  isLiked: boolean = false;

  constructor(private likeService: LikeService) {}

  ngOnInit(): void {
    this.getLikesCount();
    // Optionally, determine if the current user has already liked this recipe.
  }

  getLikesCount(): void {
    this.likeService.getLikes(this.recipeId).subscribe({
      next: (likes: Like[]) => {
        this.likesCount = likes.length;
      },
      error: (err) => console.error('Failed to get likes:', err)
    });
  }

  toggleLike(): void {
    if (this.disabled) {
      return;
    }
    const token = localStorage.getItem('token');
    if (!token) {
      alert('Please login to like this recipe.');
      return;
    }
    const userId = localStorage.getItem('userId') || 'anonymous';
    if (!this.isLiked) {
      const input: LikeInputModel = {
        recipeId: this.recipeId,
        userId: userId
      };
      this.likeService.addLike(input).subscribe({
        next: () => {
          this.isLiked = true;
          this.getLikesCount();
        },
        error: (err) => console.error('Failed to add like:', err)
      });
    } else {
      this.likeService.getLikes(this.recipeId).subscribe({
        next: (likes: Like[]) => {
          const likeForUser = likes.find(like => like.userId === userId);
          if (likeForUser) {
            this.likeService.deleteLike(likeForUser.id).subscribe({
              next: () => {
                this.isLiked = false;
                this.getLikesCount();
              },
              error: (err) => console.error('Failed to delete like:', err)
            });
          }
        },
        error: (err) => console.error('Failed to get likes for deletion:', err)
      });
    }
  }
}