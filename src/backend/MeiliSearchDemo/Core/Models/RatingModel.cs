using MapDataReader;

namespace Core.Models;

[GenerateDataReaderMapper]
public class RatingModel
{
    public int MovieId { get; set; }
    public int Rating { get; set; }
    public int Votes { get; set; }
}