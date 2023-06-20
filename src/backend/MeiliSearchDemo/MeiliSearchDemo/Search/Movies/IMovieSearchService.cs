namespace MeiliSearchDemo.Search.Movies
{
    public interface IMovieSearchService
    {
        string IndexName { get; }

        Task<IEnumerable<Movie>> GetEntries(string searchTerm, int limit);


        Task AddEntries(IEnumerable<Movie> documents);
    }
}