using System.IdentityModel.Tokens.Jwt;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IBaseService, BaseService>();
builder.Services.AddHttpClient<IBasketService,BasketService>();
builder.Services.AddHttpClient<IOrderService,OrderService>();
builder.Services.AddControllersWithViews();
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];
SD.BasketAPIBase = builder.Configuration["ServiceUrls:BasketAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService,BasketService>();
builder.Services.AddScoped<IOrderService,OrderService>();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        
        
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = SD.AuthAPIBase;
        options.ClientId = "mvc-client";
        options.ClientSecret = "mvc-client-secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("product");
        options.Scope.Add("basket");
        options.Scope.Add("order");
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//.RequireAuthorization();

app.Run();