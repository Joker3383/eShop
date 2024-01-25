namespace Catalog.API.Data;

public class DbInitializer
{
    public async static  Task Initialize(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Products.Any())
        {
            await context.AddRangeAsync(GetMockProducts());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<Product> GetMockProducts()
    {
        List<Product> mockProducts = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Name = "Product 1",
                Price = 19.99d,
                CategoryName = "Electronics",
                Description = "A fantastic electronic device.",
                ImageUrl = ""
            },
            new Product
            {
                ProductId = 2,
                Name = "Product 2",
                Price = 29.99d,
                CategoryName = "Clothing",
                Description = "Comfortable and stylish clothing.",
                ImageUrl = ""
            },
            new Product
            {
                ProductId = 3,
                Name = "Product 3",
                Price = 9.99d,
                CategoryName = "Books",
                Description = "An interesting book to read.",
                ImageUrl = ""
            }
        };
        return mockProducts;
    }

}