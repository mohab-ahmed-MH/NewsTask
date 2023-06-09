using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsTask.Api.Helpers;
using NewsTask.Core.Models;
using NewsTask.Core.Repository;
using NewsTask.EF;
using NewsTask.EF.Repositories;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddControllers();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b =>b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));



builder.Services.AddTransient(typeof(IAuthorServices),typeof(AuthorServices));
builder.Services.AddTransient(typeof(INewsServices),typeof(NewsServices));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
