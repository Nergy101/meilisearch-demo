using AutoFixture;
using MeiliSearchDemo.Search;
using MeiliSearchDemo.Search.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoFixture;

var builder = WebApplication.CreateBuilder(args);


var seeder = new MovieSearchService(Options.Create(new SearchOptions()));

await seeder.DeleteAllEntries();

var documents = new Fixture().CreateMany<Movie>(1000);

await seeder.AddEntries(documents);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var serviceInstance = new SearchService<Movie>("movies");
builder.Services.Configure<SearchOptions>((options) => options = new SearchOptions());
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/search/{searchTerm}", ([FromServices] IMovieSearchService movieSearchService, string searchTerm) =>
{
    return movieSearchService.GetEntries(searchTerm, 100);

}).WithOpenApi();

app.MapPost("movies", async ([FromServices] IMovieSearchService movieSearchService, [FromBody] IEnumerable<Movie> movies) =>
{
    await movieSearchService.AddEntries(movies);
}).WithOpenApi();

app.Run();
