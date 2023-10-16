namespace MeiliSearchDemo.Search.Movies
{
    public interface IMovieSearchService
    {
        string IndexName { get; }

        Task<int> GetNumberOfDocuments();

        Task<IEnumerable<MovieDTO>> GetEntries(string searchTerm, int limit);

        Task AddEntries(IEnumerable<MovieDTO> documents);

        Task DeleteAllEntries();
    }
}
