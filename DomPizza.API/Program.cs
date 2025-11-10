using DomPizza.Data.Context;
using DomPizza.Data.Repositories;
using DomPizza.Domain.Interfaces;
using DomPizza.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev", policy =>
        policy.WithOrigins("http://localhost:5118")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// DB + Dependências
builder.Services.AddDbContext<DomPizzaContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DomPizza API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: **Bearer {seu_token_aqui}**"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseCors("AllowBlazorDev");
app.UseAuthentication();
app.UseAuthorization();

// === ROTAS MINIMAL API ===

// Login público
app.MapPost("/login", async (IAuthService authService, DomPizza.Service.DTOs.LoginDTO dto) =>
{
    var token = await authService.AutenticarAsync(dto);
    return token is null
        ? Results.Unauthorized()
        : Results.Ok(new { token });
})
.WithName("Login")
.WithTags("Autenticação");

// Rota perfil do usuário
app.MapGet("/usuario/perfil", [Authorize] (ClaimsPrincipal user) =>
{
    var nome = user.Identity?.Name;
    var email = user.FindFirst(ClaimTypes.Email)?.Value;

    return Results.Ok(new
    {
        Nome = nome,
        Email = email,
        Mensagem = "Acesso autorizado. Bem-vindo ao seu perfil!"
    });
})
.WithName("PerfilDoUsuario")
.WithTags("Usuário");

app.Run();
