using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using StajApi.Data;
using StajApi.Services;
using StajApi.ExceptionHandling;
using StajApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// PostgreSQL Veritabanı Bağlantısı (DbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Service ve Interface Kaydı
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo //new openapiinfo swaggerda gosterilecek tanıtım belgelerini oluşturur
    {
        Title = "Staj API",
        Version = "v1",
        Description = "Stajyer kayıtlarını listelemek, eklemek, güncellemek ve silmek için hazırlanmış Web API projesidir.",
        Contact = new OpenApiContact
        {
            Name = "Kaan Kesen",
            Url = new Uri("https://github.com/kaanksn1")
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor
        | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment()) // gereksiz bilgileri dışarıya açmamak için yalnız geliştirme ortamında çalışır
{
    app.UseSwagger(); // OpenAPI JSON belgesini /swagger/v1/swagger.json adresinde yayımlar
    app.UseSwaggerUI(); // Swagger görsel ekranını /swagger adresinde açar
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
