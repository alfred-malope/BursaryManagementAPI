using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Storage.Blobs;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.Data.SqlClient;
using BusinessLogic;
using DataAccess;
// Startup.cs



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddSingleton<SqlConnection>(_ => new SqlConnection(connectionString));
builder.Services.AddScoped<StudentFundRequestDAL>();
builder.Services.AddScoped<StudentFundRequestBLL>();

// Configure Azure Blob Storage
builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var storageConnectionString = configuration.GetConnectionString("AzureStorageConnectionString");
    var blobServiceClient = new BlobServiceClient(storageConnectionString);
    var blobContainerClient = blobServiceClient;
    return blobContainerClient;
});


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BursaryManagementAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BursaryManagementAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
