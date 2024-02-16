using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Storage;
using Azure.Storage.Blobs;
using System;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddSingleton<SqlConnection>(_ => new SqlConnection(connectionString));

// Configure Azure Blob Storage
builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var storageconnectionString = configuration.GetConnectionString("AzureStorageConnectionString");
    var blobServiceClient = new BlobServiceClient(storageconnectionString);
    var blobContainerName = "bursarymanagementstorage";
    var blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
    return blobContainerClient;
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
