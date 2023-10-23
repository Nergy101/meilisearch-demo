namespace Core.Models;

public class MovieFlyweight
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? Year { get; set; }

    public RatingModel? Rating { get; set; }
}