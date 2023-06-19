using Meilisearch;

namespace MeiliSearchDemo
{
    public class SearchService<T> where T : class
    {
        private readonly MeilisearchClient _meilisearchClient;
        private readonly Meilisearch.Index _index;

        public SearchService(string indexName)
        {

            _meilisearchClient = new MeilisearchClient("http://localhost:7700", "MASTER_KEY");
            // An index is where the documents are stored.
            _index = _meilisearchClient.Index(indexName);
        }


        public async Task AddEntries(IEnumerable<T> documents)
        {

            var task = await _index.AddDocumentsAsync<T>(documents);
        }

        public async Task<IEnumerable<T>> GetEntries(string searchTerm, int limit = 300)
        {
            var results = await _index.SearchAsync<T>(searchTerm, new SearchQuery { Limit = limit });

            return results.Hits;
        }
    }
}



public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<string> Genres { get; set; }
}
