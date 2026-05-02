using HMYS.BUsiness.Interfaces;
using HMYS.BUsiness.Models;
using HMYS.Web.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using HMYS.BUsiness.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
// Mevcut builder satýrlarýnýn yanýna ekle
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Adlandýrma politikasýný null yaparak C# isimlerini aynen korur
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();

    app.MapScalarApiReference(options =>
    {
        // Scalar'a "Benim GET/POST listemi Swagger'ýn dosyasýndan al" diyoruz:
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<HMYS.Web.Middlewares.ExceptionMiddleware>();

app.Run();
