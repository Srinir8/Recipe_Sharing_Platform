export interface Like {
  id: string; // Guid
  recipeId: string; // Guid of the recipe
  userId: string; // Guid of the user
  likedAt: Date;
}