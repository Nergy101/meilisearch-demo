export interface Movie {
  id: number;
  title: string;
  year: number;
  rating: Rating
}

export interface Rating {
  movieId: number,
  rating: number,
  votes: number
}