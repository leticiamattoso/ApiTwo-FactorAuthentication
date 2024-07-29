using System.Security.Claims;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer("Server=localhost\\SQLEXPRESS,1433;Database=identity-db;User ID=sa;Password=Senha123;Trusted_Connection=False; TrustServerCertificate=True;"));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.
    AddIdentityApiEndpoints<User>().
    AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
    // app.MapSwagger().
    //     RequireAuthorization();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (ClaimsPrincipal user) => user.Identity!.Name).RequireAuthorization();

app.MapIdentityApi<User>();

//Método Post
app.MapPost(pattern: "/logout",
    async (SignInManager<User> signInManager, [FromBody] object empty) =>
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    });

app.Run();
