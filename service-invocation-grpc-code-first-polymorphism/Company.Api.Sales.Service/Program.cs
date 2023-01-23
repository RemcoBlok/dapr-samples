using Company.Framework;
using Company.Manager.Sales.Interface;
using Dapr.Client;
using ProtoBuf.Grpc.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.UseOneOfForPolymorphism();
    o.SelectDiscriminatorNameUsing(type => "$type");
    o.SelectDiscriminatorValueUsing(type => type.FullName);
    o.CustomSchemaIds(type => type.FullName);
});
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolver = new PolymorphicTypeResolver();
});
builder.Services.AddDaprClient();
builder.Services.AddSingleton(_ => DaprClient.CreateInvocationInvoker("company-manager-sales-service").CreateGrpcService<ISalesManager>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/sales/finditem", async (ISalesManager salesManager, FindCriteriaBase criteria) =>
{
    FindResponseBase response = await salesManager.FindItemAsync(criteria);
   return TypedResults.Ok(response);
})
.WithName("Sales.FindItem")
.WithOpenApi();

app.Run();
