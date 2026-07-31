using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StajApi.Data;
using StajApi.Services;
using StajApi.ExceptionHandling;
using StajApi.Models;
using StajApi.Models.Configuration;
using StajApi.Models.Entities;
using StajApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            bool hasJsonError = context.ModelState.Keys.Any(
                key => key == "$" || key.StartsWith("$.")
            );

            string message;

            if (hasJsonError)
            {
                message = "JSON formatı geçersiz. Virgül, tırnak ve parantezleri kontrol edin.";
            }
            else
            {
                message = context.ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage)
                    .FirstOrDefault(error => !string.IsNullOrWhiteSpace(error))
                    ?? "Gönderilen bilgiler geçersiz.";
            }

            return new BadRequestObjectResult(
                new ErrorResponse { Message = message }
            );
        };
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services
    .AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.Key),
        "Jwt:Key boş olamaz.")
    .Validate(settings => settings.Key.Length >= 32,
        "Jwt:Key en az 32 karakter olmalıdır.")
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer),
        "Jwt:Issuer boş olamaz.")
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience),
        "Jwt:Audience boş olamaz.")
    .ValidateOnStart();

JwtSettings jwtSettings = builder.Configuration
    .GetRequiredSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT ayarları bulunamadı.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)
            ),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization();

// PostgreSQL Veritabanı Bağlantısı (DbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Service ve Interface Kaydı
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Login endpointinden alınan JWT tokenini girin."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
