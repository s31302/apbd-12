using apbd12.Data;
using apbd12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddDbContext<Apbd12Context>(options =>
    options.UseSqlServer("Server=localhost/SQLEXPRESS;Database=Database12;Trusted_Connection=True;Encrypt=False"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();