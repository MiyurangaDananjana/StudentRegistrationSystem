using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentRegistrationSystemApi.Data;
using StudentRegistrationSystemApi.Data.Repositories.Implementations;
using StudentRegistrationSystemApi.Data.Repositories.Interfaces;
using StudentRegistrationSystemApi.Service.Implementations;
using StudentRegistrationSystemApi.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3001", "http://localhost:3000") // Add your frontend URLs here
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});



builder.Services.AddScoped<IStudentsRepositories, StudentsRepositories>();
builder.Services.AddScoped<IStudentsService, StudentsService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Enable CORS middleware
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
