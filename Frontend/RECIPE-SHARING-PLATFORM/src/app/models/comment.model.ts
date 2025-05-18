export interface Comment {
  id: string; // Guid
  recipeId: string; // Guid of the recipe
  userId: string; // Guid of the user
  content: string; // Comment content
  createdAt: Date;
}