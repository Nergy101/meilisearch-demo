namespace MeiliSearchDemo.Search.Movies;

public class MovieDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? Year { get; set; }

    public RatingModel? Rating { get; set; }
}