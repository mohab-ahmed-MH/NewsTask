using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NewsTask.Mvc.Data;
using NewsTask.Mvc.Interfaces;
using NewsTask.Mvc.Managers;
using System.Net;
using System.Text;
using Microsoft.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NewsTaskMvcContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewsTaskMvcContext") ?? throw new InvalidOperationException("Connection string 'NewsTaskMvcContext' not found.")));


builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddScoped(typeof(IAPIManager<>), typeof(APIManager<>));
builder.Services.AddScoped(typeof(IAuthAPIManager), typeof(AuthAPIManager));

builder.Services.AddHttpClient("HttpMessageHandler").AddHttpMessageHandler<NewsTask.Mvc.Handlers.TokenHandler>();
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

//app.UseStatusCodePages(async context =>
//{
//    var request = context.HttpContext.Request;
//    var response = context.HttpContext.Response;

//    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
//    {
//        response.Redirect("/Login");
//    }
//});

app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("JWT_Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
    }
    await next();
});

app.Use(async (context, next) =>
{
    var host = context.Request.Host;
    var response = context.Response;
    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/Login");
    }
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
