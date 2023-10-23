using Core.Search;
using Core.Search.Movies;
using Infrastructure;
using MeiliSearchDemo.EndpointExtensions;
using MeiliSearchDemo.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

string[] origins = {
    "http://localhost:4200",
    "http://localhost",
    "http://frontend:80"
};

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Builder environment: {builder.Environment}");

// Add Core
var searchOptionsSection = new SearchOptions();
builder.Configuration.GetSection(nameof(SearchOptions)).Bind(searchOptionsSection);
var searchOptions = Options.Create(searchOptionsSection);

builder.Services.AddSingleton(searchOptions);
builder.Services.AddTransient<IMovieSearchService, MovieSearchService>();

// Add pipeline components
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeiliSearchDemo", Version = "v1" });
});

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
DemoSeeder.Migrate();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
Task.Run(async () =>
{
    await DemoSeeder.SeedDemoDbData();
    await DemoSeeder.SeedMovieData(searchOptions);
}).ConfigureAwait(false);
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

// Maps
app.MapSwagger();

app.AddMovieEndpoints(myAllowSpecificOrigins);

app.MapHealthChecks("/health")
    .RequireCors(myAllowSpecificOrigins);

// Run
app.Run();
