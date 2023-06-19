using Meilisearch;
using AutoFixture;

var fixture = new Fixture();

var client = new MeilisearchClient("http://localhost:7700", "MASTER_KEY");

// An index is where the documents are stored.
var index = client.Index("movies");
var documents = fixture.CreateMany<Movie>(100_000);

Console.WriteLine("Adding documents to index...");
Console.WriteLine(documents.Count());
Console.WriteLine(documents.SelectMany(x => x.Genres).Count());

var task = await index.AddDocumentsAsync<Movie>(documents);
Console.WriteLine("Documents added to index");

var foundDocuments = await index.SearchAsync<Movie>("abcd", new SearchQuery { Limit = 300 });
Console.WriteLine(foundDocuments.ProcessingTimeMs);
Console.WriteLine(foundDocuments.Hits.Count());
foreach (var document in foundDocuments.Hits)
{
    Console.WriteLine(document.Title);
}

public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<string> Genres { get; set; }
}
