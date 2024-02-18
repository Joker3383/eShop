
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Order.API;
using Order.API.Data;
using Order.API.Mapping;
using Order.API.Repositories;
using Order.API.Repositories.Interfaces;
using Order.API.Services;
using Order.API.Services.Interfaces;
using Order.API.Utilities;
using Shared.CrudOperations;

var configuration = GetConfiguration();


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SD.CatalogApiBase = builder.Configuration["Urls:CatalogAPI"];
SD.AuthApiBase = builder.Configuration["Urls:AuthAPI"];
SD.BasketApiBase = builder.Configuration["Urls:BasketAPI"];
builder.Services.AddHttpClient();

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IBasketRepository, BasketRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = SD.AuthApiBase;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("AuthenteficatedUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "order");
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{SD.AuthApiBase}/connect/authorize"),
                TokenUrl = new Uri($"{SD.AuthApiBase}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "order", "OrderAPI" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "order" }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Cors",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddDbContextFactory<AppDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});



builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule<MediatorModule>());




var app = builder.Build();



app.UseCors("Cors");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
    c.OAuthClientId("order_swaggerui");
    c.OAuthAppName("Order Swagger UI");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}
