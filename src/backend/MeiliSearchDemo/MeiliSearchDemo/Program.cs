using MeiliSearchDemo.Search;
using MeiliSearchDemo.Search.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MeiliSearchDemo;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var seeder = new MovieSearchService(Options.Create(new SearchOptions()));
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SearchOptions>(o => o = new SearchOptions());
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        b => { b.WithOrigins("http://localhost:4200"); });
});
var app = builder.Build();

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
        movieSearchService.GetEntries(searchTerm, 100)
).WithOpenApi().RequireCors(myAllowSpecificOrigins);
;

//app.MapPost("movies",
//    async ([FromServices] IMovieSearchService movieSearchService, [FromBody] IEnumerable<MovieModel> movies) =>
//    {
//        await movieSearchService.AddEntries(movies);
//    }).WithOpenApi().RequireCors(myAllowSpecificOrigins);
//;

app.Run();
