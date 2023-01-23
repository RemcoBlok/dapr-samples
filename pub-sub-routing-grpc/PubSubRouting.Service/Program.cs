using MassTransit;
using PubSubRouting.Service.Consumers;
using PubSubRouting.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDaprClient();
builder.Services.AddMediator(registration =>
{
    registration.AddConsumer<InventoryConsumer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCloudEvents();
app.MapGrpcService<AppCallbackService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
