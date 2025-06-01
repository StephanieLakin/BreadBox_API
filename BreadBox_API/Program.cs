using BreadBox_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models; 
using Swashbuckle.AspNetCore.SwaggerGen; 
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://breadbox-prod.web.app",
            "https://breadbox-staging.web.app"
            )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<BreadBoxDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BreadBox API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BreadBox API v1");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at the root URL (e.g., http://localhost:5000/)
});
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();



