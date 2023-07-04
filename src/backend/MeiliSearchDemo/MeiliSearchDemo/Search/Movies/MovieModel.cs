using MapDataReader;

namespace MeiliSearchDemo.Search.Movies;

[GenerateDataReaderMapper]
public class MovieModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
}