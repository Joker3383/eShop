
var configuration = GetConfiguration();


var builder = WebApplication.CreateBuilder(args);

SD.CatalogApiBase = builder.Configuration["Urls:Catalog.API"];
SD.AuthApiBase = builder.Configuration["Urls:Auth.API"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IBasketService, BasketService>();

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
    c.OAuthClientId("basket_swaggerui");
    c.OAuthAppName("Basket Swagger UI");
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
