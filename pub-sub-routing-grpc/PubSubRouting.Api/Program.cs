using Dapr;
using Dapr.Client;
using PubSubRouting.Framework;
using PubSubRouting.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.UseOneOfForPolymorphism();
    o.SelectDiscriminatorNameUsing(type => "$type");
    o.SelectDiscriminatorValueUsing(type => type.Name);
    o.CustomSchemaIds(type => type.Name);

});
builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/publishProduct", async (DaprClient daprClient, Product product) =>
{
    CloudEvent<Product> cloudEvent = new(product)
    {
        Source = new Uri("pubsubrouting-api", UriKind.Relative),
        Type = product.GetType().GetSimpleAssemblyQualifiedName(), // must be AssemblyQualifiedName of Type, but may use simple Name of Assembly (without the Version, Culture and PublicKeyToken) instead of FullName of Assembly (with the Version, Culture and PublicKeyToken)
        Subject = "product"
    };
    await daprClient.PublishEventAsync("pubsub", "inventory", cloudEvent);
})
.WithName("PublishProduct")
.WithOpenApi();

app.MapPost("/publishNotAProduct", async (DaprClient daprClient, NotAProduct product) =>
{
    CloudEvent<NotAProduct> cloudEvent = new(product)
    {
        Source = new Uri("pubsubrouting-api", UriKind.Relative),
        Type = product.GetType().GetSimpleAssemblyQualifiedName(), // must be AssemblyQualifiedName of Type, but may use simple Name of Assembly (without the Version, Culture and PublicKeyToken) instead of FullName of Assembly (with the Version, Culture and PublicKeyToken)
        Subject = "product"
    };
    await daprClient.PublishEventAsync("pubsub", "inventory", cloudEvent);
})
.WithName("PublishNotAProduct")
.WithOpenApi();

app.Run();
