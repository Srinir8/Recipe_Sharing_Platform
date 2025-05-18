import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Recipe, RecipeInputModel } from '../models/recipe.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = `${environment.apiUrl}/Recipe`;

  constructor(private http: HttpClient) {}

  getAllRecipes(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(this.apiUrl);
  }

  getRecipeById(id: string): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.apiUrl}/${id}`);
  }

  addRecipe(recipe: RecipeInputModel): Observable<Recipe> {
    const formData = new FormData();
    formData.append('title', recipe.title);
    formData.append('description', recipe.description);
    formData.append('userId', recipe.userId);
    recipe.ingredients.forEach((ingredient, index) =>
      formData.append(`ingredients[${index}]`, ingredient)
    );
    recipe.instructions.forEach((instruction, index) =>
      formData.append(`instructions[${index}]`, instruction)
    );
    if (recipe.picture) {
      formData.append('picture', recipe.picture);
    }
    return this.http.post<Recipe>(this.apiUrl, formData);
  }

  updateRecipe(id: string, recipe: RecipeInputModel): Observable<void> {
    const formData = new FormData();
    formData.append('title', recipe.title);
    formData.append('description', recipe.description);
    formData.append('userId', recipe.userId);
    recipe.ingredients.forEach((ingredient, index) =>
      formData.append(`ingredients[${index}]`, ingredient)
    );
    recipe.instructions.forEach((instruction, index) =>
      formData.append(`instructions[${index}]`, instruction)
    );
    if (recipe.picture) {
      formData.append('picture', recipe.picture);
    }
    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }

  deleteRecipe(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}