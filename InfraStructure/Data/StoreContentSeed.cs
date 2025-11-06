using System.Text.Json;
using Core.Entities;

namespace InfraStructure.Data;

public class StoreContentSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../InfraStructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products is null) return;

            context.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
