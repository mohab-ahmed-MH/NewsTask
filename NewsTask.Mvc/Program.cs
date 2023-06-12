using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsTask.Mvc.Data;
using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Managers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NewsTaskMvcContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewsTaskMvcContext") ?? throw new InvalidOperationException("Connection string 'NewsTaskMvcContext' not found.")));

builder.Services.AddScoped(typeof(IAPIManager<>), typeof(APIManager<>));
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
