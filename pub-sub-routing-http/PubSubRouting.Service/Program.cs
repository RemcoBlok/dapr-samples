using Dapr;
using PubSubRouting.Interfaces;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCloudEvents();
//app.MapSubscribeHandler(); // using declarative subscriptions

app.MapPost("/inventory/widgets", //[Topic("pubsub", "inventory", "event.type ==\"Widget\"", 1, DeadLetterTopic = "poisonMessages")]
(Widget widget) =>
{
    Console.WriteLine("Subscriber received : " + widget);
    return Results.Ok();
});

app.MapPost("/inventory/gadgets", //[Topic("pubsub", "inventory", "event.type ==\"Gadget\"", 2, DeadLetterTopic = "poisonMessages")]
(Gadget gadget) =>
{
    Console.WriteLine("Subscriber received : " + gadget);
    return Results.Ok();
});

app.MapPost("/inventory/products", //[Topic("pubsub", "inventory", DeadLetterTopic = "poisonMessages")]
(Product product) =>
{
    Console.WriteLine("Subscriber received (default) : " + product);
    return Results.Ok();
});

app.MapPost("/failedMessages", //[Topic("pubsub", "poisonMessages")]
(JsonDocument message) =>
{
    Console.WriteLine("Subscriber received (dead letter) : " + message);
    return Results.Ok();
});

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
