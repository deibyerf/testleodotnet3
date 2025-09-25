using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static IEnumerable<string> GetIPs() =>
    Dns.GetHostEntry(Dns.GetHostName())
       .AddressList
       .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
       .Select(ip => ip.ToString());

app.MapGet("/ping", () => Results.Ok(new {
    service = "HelloApi",
    msg = "pong",
    container = Environment.MachineName,
    ips = GetIPs()
}));

app.MapGet("/whoami", () => Results.Ok(new {
    service = "HelloApi",
    container = Environment.MachineName,
    ips = GetIPs(),
    time = DateTimeOffset.UtcNow
}));

app.Run();