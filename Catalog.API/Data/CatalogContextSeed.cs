using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        bool existsProduct = productCollection.Find(p => true).Any();

        if (!existsProduct)
        {
            productCollection.InsertManyAsync(GetMyProducts());
        }
    }

    public static IEnumerable<Product> GetMyProducts()
    {
        return new List<Product>()
        {
            new Product()
            {
                Id = "645fabd19c5e13a11caa5a51",
                Name = "Iphone X",
                Description = "Descrição detalhada do produto Iphone X",
                Image = "product-1.png",
                Price = 950.00M,
                Category = "Smartphone"
            },
            new Product()
            {
                Id = "645facab8b2a329db0178e8d",
                Name = "Notebook Lenovo",
                Description = "Descrição detalhada do produto Notebook Lenovo",
                Image = "product-2.png",
                Price = 1250.00M,
                Category = "Eletrônicos"
            },
            new Product()
            {
                Id = "645facbfccacbad93115ddc2",
                Name = "Fone Bluetooth",
                Description = "Descrição detalhada do produto Fone Bluetooth",
                Image = "product-3.png",
                Price = 500.00M,
                Category = "Eletrônicos"
            },
            new Product()
            {
                Id = "645facd47637bad970c73e16",
                Name = "Jogo de Facas Tramontina",
                Description = "Descrição detalhada do produto Jogo de Facas Tramontina",
                Image = "product-4.png",
                Price = 250.00M,
                Category = "Itens para cozinha"
            },
            new Product()
            {
                Id = "645fad01bcabcc92a29f99f0",
                Name = "Panela de pressão Polishop",
                Description = "Panela de pressão Polishop",
                Image = "product-5.png",
                Price = 600.00M,
                Category = "Itens para cozinha"
            },
        };
    }
}