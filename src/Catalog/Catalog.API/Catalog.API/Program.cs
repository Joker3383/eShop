using Autofac;
using Autofac.Extensions.DependencyInjection;
using Catalog.API;
using Catalog.API.Data;
using Catalog.API.Mapping;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.API.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.CrudOperations;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBaseRepository, BaseRepository>();
builder.Services.AddTransient<IProductService, ProductService>();


foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContextFactory<AppDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));

var authority = builder.Configuration["Authorization:Authority"];

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("AuthenteficatedUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "product");
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "catalog", "BffController" },
                    { "product", "ProductController" }
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
            new[] { "catalog", "product" }
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
    c.OAuthClientId("swaggerui");
    c.OAuthAppName("Swagger UI");
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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