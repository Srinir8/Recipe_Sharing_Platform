import { Routes } from '@angular/router';
import { RecipeComponent } from './components/recipe/recipe.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  { path: '', redirectTo: '/recipes', pathMatch: 'full' },
  { path: 'recipes', component: RecipeComponent },
  { path: 'users', component: UserComponent },
  { path: 'login', component: LoginComponent }
];