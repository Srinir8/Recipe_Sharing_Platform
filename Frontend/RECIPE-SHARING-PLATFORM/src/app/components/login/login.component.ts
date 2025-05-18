import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from '../../services/user.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  returnTo: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['returnTo']) {
        this.returnTo = params['returnTo'];
      }
      const token = params['token'];
      if (token) {
        localStorage.setItem('token', token);
        this.userService.clearCache();
        if (this.returnTo === 'addRecipe') {
          this.router.navigate(['/recipes'], { queryParams: { addRecipe: true } });
        } else if (this.returnTo === 'profile') {
          this.router.navigate(['/users']);
        } else {
          this.router.navigate(['/']);
        }
      }
    });
  }

  get isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  loginWithGoogle(): void {
    // Use environment variable for the auth URL
    window.location.href = `${environment.Url}/auth/google`;
  }

  logout(): void {
    localStorage.removeItem('token');
    this.userService.clearCache();
    this.router.navigate(['/login']);
  }
}