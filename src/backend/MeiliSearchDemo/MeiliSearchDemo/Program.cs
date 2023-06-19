using AutoFixture;
using MeiliSearchDemo;
using Microsoft.AspNetCore.Mvc;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serviceInstance = new SearchService<Movie>("movies");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/search/{searchTerm}", (string searchTerm) =>
{

    return serviceInstance.GetEntries(searchTerm, 100);

}).WithOpenApi();

app.MapPost("movies", async ([FromBody] IEnumerable<Movie> movies) =>
{
    await serviceInstance.AddEntries(movies);
}).WithOpenApi();

app.Run();
