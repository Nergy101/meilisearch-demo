using Core.Models;

namespace Core.Search.Movies
{
    public interface IMovieSearchService
    {
        string IndexName { get; }

        Task<int> GetNumberOfDocuments();

        Task<IEnumerable<MovieFlyweight>> GetEntries(string searchTerm, int limit);

        Task AddEntries(IEnumerable<MovieFlyweight> documents);

        Task DeleteAllEntries();
    }
}
