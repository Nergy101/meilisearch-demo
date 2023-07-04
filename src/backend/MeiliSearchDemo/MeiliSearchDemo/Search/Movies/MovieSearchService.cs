using Microsoft.Extensions.Options;

namespace MeiliSearchDemo.Search.Movies
{
    public class MovieSearchService : SearchService<MovieDTO>, IMovieSearchService
    {
        public MovieSearchService(IOptions<SearchOptions> options) : base(options)
        {
        }

        public override string IndexName => "movies";
    }
}
