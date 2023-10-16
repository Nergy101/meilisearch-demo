using MapDataReader;
using MeiliSearchDemo.Search.Movies;
using Microsoft.Data.Sqlite;

namespace MeiliSearchDemo;

public static class SqliteReader
{
    public static (List<MovieModel>, List<RatingModel>) ReadFromPath(string pathToDb)
    {
        using var connection = new SqliteConnection($"Data Source={pathToDb}");

        connection.Open();

        return (GetMovies(connection), GetRatings(connection));
    }

    private static List<MovieModel> GetMovies(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT id, title, year FROM movies";
        using var reader = command.ExecuteReader();
        return reader.ToMovieModel();
    }

    private static List<RatingModel> GetRatings(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT movie_id movieId, rating, votes FROM ratings";
        using var reader = command.ExecuteReader();
        return reader.ToRatingModel();
    }
}
