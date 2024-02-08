

using Basket.API.Data;
using Basket.API.Mapping;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Basket.API.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
SD.CatalogApiBase = builder.Configuration["Urls:Catalog.API"];
SD.AuthApiBase = builder.Configuration["Urls:Auth.API"];


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
        policy.RequireClaim("scope", "basket");
    });
});
builder.Services.AddHttpClient();
builder.Services.AddTransient<IBasketRepository, BasketRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });

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
                    { "basket", "BasketAPI" }
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
            new[] { "basket" }
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

var app = builder.Build();



app.UseCors("Cors");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
    c.OAuthClientId("basket_swaggerui");
    c.OAuthAppName("Basket Swagger UI");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseHttpsRedirection();
CreateDbIfNotExists(app);

app.Run();



IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();

            DbInitializer.Initialize(context).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
