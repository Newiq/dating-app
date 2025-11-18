using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<API.Data.AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddScoped<API.Interfaces.ITokenServices, API.Services.TokenServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
          .AllowAnyMethod()
          .AllowAnyHeader());

app.MapControllers();

app.Run();
