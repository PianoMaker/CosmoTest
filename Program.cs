using CosmoTest.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Replace with your actual Cosmos DB credentials
var accountEndpoint = builder.Configuration["Cosmos:Endpoint"];
var accountKey = builder.Configuration["Cosmos:Key"];
var databaseName = "TestCosmosDb";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseCosmos(accountEndpoint, accountKey, databaseName));

builder.Services.Configure<CosmosSettings>(
    builder.Configuration.GetSection("Cosmos"));

var app = builder.Build();

// 🔄 Асинхронна ініціалізація бази даних
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync(); //
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
