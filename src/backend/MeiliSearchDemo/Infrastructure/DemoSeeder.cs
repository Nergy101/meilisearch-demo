using Core.Models;
using Core.Search;
using Core.Search.Movies;
using DbUp;
using DbUp.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text;

namespace Infrastructure
{
    public static class DemoSeeder
    {
        private const string seedDataConnectionString = "./assets/movies.db";
        private const string demoDbConnectionString = "Data Source=./assets/demo.db";

        public static async Task SeedMovieData(IOptions<SearchOptions> searchOptions)
        {
            var seeder = new MovieSearchService(searchOptions);
            await seeder.DeleteAllEntries();

            var (movies, ratings) = SqliteReader.ReadFromPath(seedDataConnectionString);

            var documentCount = 1_000;

            var uploadTasks = new List<Task>();
            Console.WriteLine($"Beginning to seed {documentCount} documents");

            movies.Take(documentCount)
                .Chunk(documentCount / 10).ToList()
                .ForEach(moviesChunk =>
                {
                    var moviesToUpload = moviesChunk.Select(movie => new MovieFlyweight
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Year = movie.Year,
                        Rating = ratings.SingleOrDefault(r => r.MovieId == movie.Id)
                    });

                    uploadTasks.Add(seeder.AddEntries(moviesToUpload));
                });

            Console.WriteLine($"Seeding...");

            await Task.WhenAll(uploadTasks);

            Console.WriteLine($"Finished seeding {documentCount} documents!");
        }

        public static async Task SeedDemoDbData()
        {
            var (movies, ratings) = SqliteReader.ReadFromPath(seedDataConnectionString);
            using var connection = new SqliteConnection($"{demoDbConnectionString}");

            connection.Open();

            var demoMovies = movies.Take(5);

            var cmdStringBuilder = new StringBuilder();
            cmdStringBuilder.Append("INSERT INTO Movies VALUES");

            foreach (var movie in demoMovies)
            {

                cmdStringBuilder.Append($"({movie.Id}, '{movie.Title}', {movie.Year})");
                if (movie != demoMovies.Last())
                {
                    cmdStringBuilder.Append(",");
                }
                cmdStringBuilder.AppendLine();
            }


            var cmd = cmdStringBuilder.ToString();
            var command = connection.CreateCommand();
            command.CommandText = cmd.Remove(cmd.Length - 2);
            await command.ExecuteNonQueryAsync();

            Console.WriteLine("Seeded DEMO DB with 5 records");
        }

        /// <summary>
        /// for SQL server/postgres/etc. databases. Not needed for SQLite:
        /// EnsureDatabase.For.SqlDatabase(demoConnectionString);
        /// </summary>
        public static void Migrate()
        {
            var upgradeEngine = DeployChanges.To
                .SQLiteDatabase(demoDbConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .LogScriptOutput()
                .JournalToSQLiteTable("MigrationHistory")
                .Build();

            if (upgradeEngine.IsUpgradeRequired())
            {
                upgradeEngine.PerformUpgrade();
            }
        }
    }
}
