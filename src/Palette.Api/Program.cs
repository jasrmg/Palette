using Microsoft.EntityFrameworkCore;
using Palette.Application.Features.Auth.Commands;
using Palette.Application.Interfaces;
using Palette.Infrastructure.Data;
using Palette.Infrastructure.Repositories;
using Palette.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddDbContext<PaletteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// repository services
builder.Services.AddScoped<IUserRepository, UserRepository>();

// application services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService>(provider =>
    new JwtTokenService(
        secretKey: builder.Configuration["JwtSettings:SecretKey"]!,
        issuer: builder.Configuration["JwtSettings:Issuer"]!,
        audience: builder.Configuration["JwtSettings:Audience"]!
    ));

// mediatr registration
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
