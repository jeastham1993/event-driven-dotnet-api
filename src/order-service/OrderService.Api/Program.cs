using Amazon.DynamoDBv2;
using Amazon.EventBridge;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OrderService.Api.Events;
using OrderService.Api.Infrastructure;
using OrderService.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var systemsManagerClient = new AmazonSimpleSystemsManagementClient();
var eventBridgeParameter = systemsManagerClient.GetParameterAsync(new GetParameterRequest()
{
    Name = builder.Configuration["EventBusSSMParameterName"]
}).Result;
var tableNameParameter = systemsManagerClient.GetParameterAsync(new GetParameterRequest()
{
    Name = builder.Configuration["TableSSMParameterName"]
}).Result;

Environment.SetEnvironmentVariable("EVENT_BUS_NAME", eventBridgeParameter.Parameter.Value);
Environment.SetEnvironmentVariable("TABLE_NAME", tableNameParameter.Parameter.Value);

Console.WriteLine(Environment.GetEnvironmentVariable("TABLE_NAME"));

builder.Services.AddSingleton<AmazonDynamoDBClient>(new AmazonDynamoDBClient());
builder.Services.AddSingleton<AmazonEventBridgeClient>(new AmazonEventBridgeClient());
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IEventBus, EventBridgeEventBus>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => {
    return "";
});

app.MapControllers();

app.Run();