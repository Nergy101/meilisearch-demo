using Core.Search.Movies;
using Microsoft.AspNetCore.Mvc;

namespace MeiliSearchDemo.EndpointExtensions
{
    public static class MovieEndpointExtensions
    {

        public static void AddMovieEndpoints(this WebApplication app, string corsPolicyName)
        {

            app.MapGet("/movies/search",
                ([FromServices] IMovieSearchService movieSearchService, [FromQuery] string query) =>
                    movieSearchService.GetEntries(query, 100))
                .RequireCors(corsPolicyName)
                .WithName("Search")
                .WithOpenApi();

            app.MapGet("/movies/count",
                ([FromServices] IMovieSearchService movieSearchService) =>
                    movieSearchService.GetNumberOfDocuments())
                .RequireCors(corsPolicyName)
                .WithName("MoviesCount")
                .WithOpenApi();
        }
    }
}
