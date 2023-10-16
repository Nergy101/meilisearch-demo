using MeiliSearchDemo;
using MeiliSearchDemo.Search;
using MeiliSearchDemo.Search.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Is development: {builder.Environment.IsDevelopment()}");

var searchOptions = new SearchOptions
{
    SearchApiKey = "_8ru_e8rwXWpeVZgAr-P8V_8wCFzQYb99AHWB52jzFY"
};

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);

    searchOptions.SearchEndpoint = "http://localhost:7700";
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json");
    searchOptions.SearchEndpoint = "http://search:7700";
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#pragma warning disable S1854 // Unused assignments should be removed
builder.Services.Configure<SearchOptions>(o => o = searchOptions);
#pragma warning restore S1854 // Unused assignments should be removed
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        b =>
        {
            b.WithOrigins("http://localhost:4200", "http://frontend:80");
        });
});

var app = builder.Build();

// Seed data
var seeder = new MovieSearchService(Options.Create(searchOptions));
await seeder.DeleteAllEntries();

var (movies, ratings) = SqliteReader.ReadFromPath("./assets/movies.db");

var documents = movies.Take(10_000).Select(movie =>
{
    return new MovieDTO
    {
        Id = movie.Id,
        Title = movie.Title,
        Year = movie.Year,
        Rating = ratings.SingleOrDefault(r => r.MovieId == movie.Id)
    };
}).ToList();

await seeder.AddEntries(documents);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapGet("/search/{searchTerm}",
    ([FromServices] IMovieSearchService movieSearchService, string searchTerm) =>
        movieSearchService.GetEntries(searchTerm, 100))
    .WithOpenApi()
    .RequireCors(myAllowSpecificOrigins);

//app.MapPost("movies",
//    async ([FromServices] IMovieSearchService movieSearchService, [FromBody] IEnumerable<MovieModel> movies) =>
//    {
//        await movieSearchService.AddEntries(movies);
//    }).WithOpenApi().RequireCors(myAllowSpecificOrigins);
//;

app.Run();
