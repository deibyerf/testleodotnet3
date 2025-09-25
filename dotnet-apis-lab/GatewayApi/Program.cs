using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// URLs internas hacia los otros servicios
var helloUrl = builder.Configuration["HELLO_URL"] ?? "http://helloapi:8080";
var timeUrl  = builder.Configuration["TIME_URL"]  ?? "http://timeapi:8080";

var app = builder.Build();

app.MapGet("/", () => "Bienvenido a GatewayApi 👋");

app.MapGet("/hello", async () =>
{
    using var http = new HttpClient();
    return await http.GetFromJsonAsync<dynamic>($"{helloUrl}/ping");
});

app.MapGet("/time", async () =>
{
    using var http = new HttpClient();
    return await http.GetFromJsonAsync<dynamic>($"{timeUrl}/now");
});

app.MapGet("/status", async () =>
{
    using var http = new HttpClient();
    var hello = await http.GetFromJsonAsync<dynamic>($"{helloUrl}/whoami");
    var time  = await http.GetFromJsonAsync<dynamic>($"{timeUrl}/now");
    return Results.Ok(new { gateway = Environment.MachineName, hello, time });
});

app.Run();