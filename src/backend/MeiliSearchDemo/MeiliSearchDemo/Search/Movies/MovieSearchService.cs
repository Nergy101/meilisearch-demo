using Microsoft.Extensions.Options;

namespace MeiliSearchDemo.Search.Movies
{
    public class MovieSearchService : SearchService<Movie>, IMovieSearchService
    {
        public MovieSearchService(IOptions<SearchOptions> options) : base(options)
        { }

        public override string IndexName => "Movies";
    }

    public class Movie
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required IEnumerable<string> Genres { get; set; }
    }
}
