export interface User {
  id: string; // Guid
  googleId: string;
  email: string;
  name?: string;
  profilePictureUrl?: string;
  role: string; // Default: "User"
  createdAt: Date;
}