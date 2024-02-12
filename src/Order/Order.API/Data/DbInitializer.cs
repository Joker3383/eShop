using Order.API.Data;
using Order.API.Models.Dto;

public static class DbInitializer
{
    /*public async static  Task Initialize(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Orders.Any())
        {
            await context.AddRangeAsync(GetMockCarts());

            await context.SaveChangesAsync();
        }
    }
    */

    /*private static IEnumerable<Order.API.Models.Order> GetMockCarts()
    {
        List<Order.API.Models.Order> mockProducts = new List<Order.API.Models.Order>
        {
            new Order.API.Models.Order{
                Login = "bob",
                DateOfOrder = DateTime.Now,
                TotalSum = 751.54d,
                ShoppingCarts = new []
                {
                    new ShoppingCartDto
                    {
                        Login = "bob",
                        ProductId = 1
                    },
                    new ShoppingCartDto
                    {
                        Login = "bob",
                        ProductId = 2
                    },
                    new ShoppingCartDto
                    {
                        Login = "bob",
                        ProductId = 3
                    }
                }
            }
        };
        return mockProducts;
    }*/

}