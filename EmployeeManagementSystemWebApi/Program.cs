using EmployeeManagementSystemWebApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = null,
        WriteIndented = true,
        TypeInfoResolver = JsonSerializerOptions.Default.TypeInfoResolver
    }));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
