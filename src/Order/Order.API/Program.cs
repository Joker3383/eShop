
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Mapping;
using Order.API.Repositories;
using Order.API.Repositories.Interfaces;
using Order.API.Services;
using Order.API.Services.Interfaces;
using Order.API.Utilities;

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
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

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
app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();
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