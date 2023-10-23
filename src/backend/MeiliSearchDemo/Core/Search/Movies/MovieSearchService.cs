using Core.Models;
using Microsoft.Extensions.Options;

namespace Core.Search.Movies
{
    public class MovieSearchService : SearchService<MovieFlyweight>, IMovieSearchService
    {
        public MovieSearchService(IOptions<SearchOptions> options) : base(options)
        {
        }

        public override string IndexName => "movies";
    }
}
