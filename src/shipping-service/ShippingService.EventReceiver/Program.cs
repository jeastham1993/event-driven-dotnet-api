using Amazon.DynamoDBv2;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Mvc;
using ShippingService.EventReceiver.Infrastructure;
using ShippingService.EventReceiver.Models;

var builder = WebApplication.CreateBuilder(args);

var systemsManagerClient = new AmazonSimpleSystemsManagementClient();
var tableNameParameter = systemsManagerClient.GetParameterAsync(new GetParameterRequest()
{
    Name = builder.Configuration["TableSSMParameterName"]
}).Result;

Environment.SetEnvironmentVariable("TABLE_NAME", tableNameParameter.Parameter.Value);

builder.Services.AddSingleton<AmazonDynamoDBClient>(new AmazonDynamoDBClient());
builder.Services.AddSingleton<IShippingRepository, ShippingRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
