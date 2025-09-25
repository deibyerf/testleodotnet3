using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// URL de HelloApi dentro de la red de Docker (se puede sobreescribir por env)
var helloUrl = builder.Configuration["HELLO_URL"] ?? "http://helloapi:8080";

var app = builder.Build();

app.MapGet("/now", () =>
{
    return Results.Ok(new {
        service = "TimeApi",
        container = Environment.MachineName,
        now = DateTimeOffset.UtcNow
    });
});

app.MapGet("/hello-through-time", async () =>
{
    using var http = new HttpClient(); // conexión nueva para ver cambios entre réplicas
    var resp = await http.GetFromJsonAsync<dynamic>($"{helloUrl}/whoami");
    return Results.Ok(new {
        caller = "TimeApi",
        callerContainer = Environment.MachineName,
        hello = resp
    });
});

app.Run();