export interface RatingDto {
  ratingId: string;
  rideId: string;
  userId: string;
  rating: number;
  comments?: string;
  timestamp: string;
}
