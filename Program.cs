using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Technical Test API",
        Version = "v1",
        Description = "API para gestión de productos - Technical Test SSR"
    });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TechnicalTestDbContext>(opt => 
    opt.UseMySql(connectionString,
        new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))
    )
);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evoltis API");
        c.RoutePrefix = string.Empty; 
    });
}


app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
