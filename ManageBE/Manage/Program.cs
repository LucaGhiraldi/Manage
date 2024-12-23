using System;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json.Serialization;
using Manage;
using Manage.Data;
using Manage.Models;
using Manage.Repository.CategoriaDocumenti;
using Manage.Repository.Documenti;
using Manage.Repository.FileDocumenti;
using Manage.Service.CategoriaDocumenti;
using Manage.Service.Documenti;
using Manage.Service.FileDocumenti;
using Manage.Service.NetWorth;
using Manage.Shared;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ottieni la stringa di connessione dal file di configurazione
var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");
// Configura Entity Framework con MariaDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Identity
builder.Services.AddIdentityApiEndpoints<Utente>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    // Imposta il numero di tentativi falliti prima che l'account venga bloccato
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = false; // disabilita il blocco per i nuovi utenti
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Send grid
builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration["SendGrid:SendGridKey"]!;
});
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add services to the container.
// Configurazione dei servizi del Repository e del Service
builder.Services.AddScoped<IDocumentiRepository, DocumentiRepository>();
builder.Services.AddScoped<IDocumentiService, DocumentiService>();
builder.Services.AddScoped<IFileDocumentiRepository, FileDocumentiRepository>();
builder.Services.AddScoped<IFileDocumentiService, FileDocumentiService>();
builder.Services.AddScoped<ICategoriaDocumentiService, CategoriaDocumentiService>();
builder.Services.AddScoped<ICategoriaDocumentiRepository, CategoriaDocumentiRepository>();
builder.Services.AddScoped<FileShared>();

// Net Worth
builder.Services.AddScoped<INetWorthService, NetWorthService>();
// Configura MarketDataService con HttpClient e API Key
builder.Services.AddHttpClient<MarketDataService>((serviceProvider, httpClient) =>
{
    // Configura la BaseAddress
    //httpClient.BaseAddress = new Uri("https://finnhub.io/api/v1/");

    // Recupera la configurazione
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var apiKeyFinnhub = configuration["Finnhub:ApiKey"];
    var apiKeyAlphaVantage = configuration["AlphaVantage:ApiKey"];
    var apiKeyFinancialmodelingprep = configuration["Financialmodelingprep:ApiKey"];
    // Aggiungi l'API Key come parametro predefinito se necessario
    //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Configura un HttpClientHandler personalizzato (ad esempio per i certificati, proxy, etc.)
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5)); // Tempo di vita del gestore HTTP (opzionale)
// Aggiungi il servizio 'MarketDataService' come scoped
builder.Services.AddScoped<MarketDataService>(serviceProvider =>
{
    var httpClient = serviceProvider.GetRequiredService<HttpClient>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var apiKeyFinnhub = configuration["Finnhub:ApiKey"];
    var apiKeyAlphaVantage = configuration["AlphaVantage:ApiKey"];
    var apiKeyFinancialmodelingprep = configuration["Financialmodelingprep:ApiKey"];

    return new MarketDataService(httpClient, apiKeyFinnhub, apiKeyAlphaVantage, apiKeyFinancialmodelingprep); // Passa HttpClient e ApiKey al costruttore
});

builder.Services.AddControllers();
                //.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // Ignore JsonLoopReference

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Configurazione bearer per il funzionamento all'interno dello swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter proper JWT token",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});

// Abilitare CORS se necessario
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .WithOrigins("http://localhost:4200")
               .WithExposedHeaders("Content-Disposition");  // Espone l'header Content-Disposition al client
    });
    //options.AddPolicy("_myAllowSpecificOrigins",
    //    builder =>
    //    {
    //        builder.AllowAnyOrigin()
    //               .AllowAnyMethod()
    //               .AllowAnyHeader()
    //               .WithExposedHeaders("Content-Disposition");  // Espone l'header Content-Disposition al client
    //    });
});

// Configura l'autenticazione JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
    options.DefaultScheme = IdentityConstants.BearerScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], 
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Autorizzazione bearer
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cors
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<Utente>();

app.Run();
