import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  currentUser: User | null = null;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.fetchCurrentUser();
  }

  fetchCurrentUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (user: User) => {
        this.currentUser = user;
        // Store the user id in localStorage for later use
        localStorage.setItem('userId', user.id);
      },
      error: err => console.error('Failed to fetch current user:', err)
    });
  }

  // Navigate to login if user is not logged in.
  login(): void {
    this.router.navigate(['/login']);
  }
}