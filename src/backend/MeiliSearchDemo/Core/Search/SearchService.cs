using Meilisearch;
using Microsoft.Extensions.Options;

namespace Core.Search
{
    public abstract class SearchService<T> where T : class
    {
        private readonly MeilisearchClient _meilisearchClient;
        private readonly Meilisearch.Index _index;

        public SearchService(IOptions<SearchOptions> options)
        {
            _meilisearchClient = new MeilisearchClient(options.Value.SearchEndpoint, options.Value.SearchApiKey);
            _index = _meilisearchClient.Index(IndexName);
        }

        public abstract string IndexName { get; }

        public async Task AddEntries(IEnumerable<T> documents)
        {
            await _index.AddDocumentsAsync(documents);
        }

        public async Task<int> GetNumberOfDocuments()
        {
            var stats = await _index.GetStatsAsync();
            return stats.NumberOfDocuments;
        }

        public async Task<IEnumerable<T>> GetEntries(string searchTerm, int limit = 100)
        {
            var results = await _index.SearchAsync<T>(searchTerm, new SearchQuery
            {
                Limit = limit,
                MatchingStrategy = "last" // all || last
            });

            return results.Hits;
        }

        public async Task DeleteAllEntries()
        {
            await _index.DeleteAllDocumentsAsync();
        }
    }
}
