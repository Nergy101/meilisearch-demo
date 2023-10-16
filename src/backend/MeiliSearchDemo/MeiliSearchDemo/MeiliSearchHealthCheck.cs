using MeiliSearchDemo.Search.Movies;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MeiliSearchDemo
{
    public class MeiliSearchHealthCheck : IHealthCheck
    {
        private readonly IMovieSearchService _movieSearchService;

        public MeiliSearchHealthCheck(IMovieSearchService movieSearchService)
        {
            _movieSearchService = movieSearchService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            if ((await _movieSearchService.GetEntries(string.Empty, 1)).Any())
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
