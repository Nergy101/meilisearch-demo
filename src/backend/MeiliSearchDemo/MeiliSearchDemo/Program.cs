using MeiliSearchDemo;
using MeiliSearchDemo.Search;
using MeiliSearchDemo.Search.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
string[] origins = {
    "http://localhost:4200",
    "http://localhost",
    "http://frontend:80"
};

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Is development: {builder.Environment.IsDevelopment()}");

var searchOptions = new SearchOptions
{
    SearchApiKey = "_8ru_e8rwXWpeVZgAr-P8V_8wCFzQYb99AHWB52jzFY"
};

if (builder.Environment.IsDevelopment())
{
    // builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);

    searchOptions.SearchEndpoint = "http://localhost:7700";
}
else
{
    // builder.Configuration.AddJsonFile("appsettings.json");
    searchOptions.SearchEndpoint = "http://search:7700";
}

var iSearchOptions = Options.Create(searchOptions);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeiliSearchDemo", Version = "v1" });
});

#pragma warning disable S1854 // Unused assignments should be removed
builder.Services.AddSingleton<IOptions<SearchOptions>>(iSearchOptions);
#pragma warning restore S1854 // Unused assignments should be removed
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        b =>
        {
            b.WithOrigins(origins);
        });
});

builder.Services.AddHealthChecks()
    .AddCheck<MeiliSearchHealthCheck>(nameof(MeiliSearchHealthCheck));

var app = builder.Build();

// Seed data

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
Task.Run(SeedData).ConfigureAwait(false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeiliSearch Demo API V1");
});

app.MapSwagger();

app.MapGet("/search",
    ([FromServices] IMovieSearchService movieSearchService, [FromQuery] string query) =>
        movieSearchService.GetEntries(query, 100))
    .RequireCors(myAllowSpecificOrigins)
    .WithName("Search")
    .WithOpenApi();

app.MapGet("/movies/count",
    ([FromServices] IMovieSearchService movieSearchService) =>
        movieSearchService.GetNumberOfDocuments())
    .RequireCors(myAllowSpecificOrigins)
    .WithName("MoviesCount")
    .WithOpenApi();

app.MapHealthChecks("/health")
    .RequireCors(myAllowSpecificOrigins);

app.Run();

async Task SeedData()
{
    var seeder = new MovieSearchService(iSearchOptions);
    await seeder.DeleteAllEntries();

    var (movies, ratings) = SqliteReader.ReadFromPath("./assets/movies.db");

    var documentCount = 100_000;

    var uploadTasks = new List<Task>();
    Console.WriteLine($"Beginning to seed {documentCount} documents");

    movies.Take(documentCount)
        .Chunk(documentCount / 10).ToList()
        .ForEach(moviesChunk =>
        {
            var moviesToUpload = moviesChunk.Select(movie => new MovieDTO
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

    Console.WriteLine($"Finished seeding {documentCount} documents");
}
