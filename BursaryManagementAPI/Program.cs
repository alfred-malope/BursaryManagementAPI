using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Azure.Storage.Blobs;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddSingleton<SqlConnection>(_ => new SqlConnection(connectionString));

// Configure Azure Blob Storage
builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var storageConnectionString = configuration.GetConnectionString("AzureStorageConnectionString");
    var blobServiceClient = new BlobServiceClient(storageConnectionString);
    /* var blobContainerName = "bursarymanagementcontainer";
     var blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);*/
    var blobContainerClient = blobServiceClient;
    return blobContainerClient;
});


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BursaryManagementAPI", Version = "v1" });
});

var app = builder.Build();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
