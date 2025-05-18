import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeService } from '../../services/recipe.service';
import { Recipe, RecipeInputModel } from '../../models/recipe.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { LikeComponent } from '../like/like.component';
import { CommentComponent } from '../comment/comment.component';

@Component({
  selector: 'app-recipe',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
    MatListModule,
    LikeComponent,
    CommentComponent
  ],
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css']
})
export class RecipeComponent implements OnInit {
  recipes: Recipe[] = [];
  showAddRecipeUI: boolean = false;

  // newRecipe holds the textual fields; picture will be sent as a File.
  newRecipe: RecipeInputModel = {
    title: '',
    description: '',
    ingredients: [],
    instructions: [],
    userId: localStorage.getItem('userId') || '',
    picture: undefined
  };

  // Comma-separated input fields.
  ingredientsInput: string = '';
  instructionsInput: string = '';

  // Holds the selected picture file.
  selectedFile: File | null = null;

  constructor(
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['addRecipe'] === 'true') {
        this.showAddRecipeUI = true;
      }
    });
    this.fetchRecipes();
  }

  get isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  fetchRecipes(): void {
    this.recipeService.getAllRecipes().subscribe({
      next: (data: Recipe[]) => (this.recipes = data),
      error: err => console.error('Failed to fetch recipes:', err)
    });
  }

  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;
    const file = fileInput.files?.[0];
    if (file) {
      // Store the file object directly.
      this.selectedFile = file;
    }
  }

  addRecipe(): void {
    // Process comma-separated ingredients and instructions.
    if (this.ingredientsInput) {
      this.newRecipe.ingredients = this.ingredientsInput.split(',').map(item => item.trim());
    }
    if (this.instructionsInput) {
      this.newRecipe.instructions = this.instructionsInput.split(',').map(item => item.trim());
    }
    // Ensure userId is set.
    this.newRecipe.userId = localStorage.getItem('userId') || '';
    // Assign the selected file to picture if available.
    if (this.selectedFile) {
      this.newRecipe.picture = this.selectedFile;
    }

    // Call the service method with a RecipeInputModel object.
    this.recipeService.addRecipe(this.newRecipe).subscribe({
      next: () => {
        // Reset form and refresh recipes.
        this.newRecipe = {
          title: '',
          description: '',
          ingredients: [],
          instructions: [],
          userId: localStorage.getItem('userId') || '',
          picture: undefined
        };
        this.ingredientsInput = '';
        this.instructionsInput = '';
        this.selectedFile = null;
        this.fetchRecipes();
        this.showAddRecipeUI = false;
      },
      error: err => console.error('Failed to add recipe:', err)
    });
  }

  // Helper to determine if current user already liked the recipe.
  isRecipeLiked(recipe: Recipe): boolean {
    const userId = localStorage.getItem('userId');
    return userId ? recipe.likedBy.includes(userId) : false;
  }

  // Delete option removed per requirements.
  // toggleAddRecipe opens the Add Recipe form.
  toggleAddRecipe(): void {
    if (this.isLoggedIn) {
      this.showAddRecipeUI = true;
      this.newRecipe.userId = localStorage.getItem('userId') || '';
    } else {
      this.router.navigate(['/login'], { queryParams: { returnTo: 'addRecipe' } });
    }
  }
}