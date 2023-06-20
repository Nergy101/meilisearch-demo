using MeiliSearchDemo.Search;
using MeiliSearchDemo.Search.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


//var test = new MovieSearchService();

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
