export interface Recipe {
  id: string; // Guid
  title: string;
  description: string;
  ingredients: string[];
  instructions: string[];
  userId: string; // User ID of the creator
  likedBy: string[]; // List of user IDs who liked the recipe
  comments: Comment[]; // List of comments
  picture?: string; // Base64-encoded image or URL
  createdAt: Date;
  updatedAt: Date;
}

export interface RecipeInputModel {
  title: string;
  description: string;
  ingredients: string[];
  instructions: string[];
  userId: string;
  picture?: File; // Image file
}