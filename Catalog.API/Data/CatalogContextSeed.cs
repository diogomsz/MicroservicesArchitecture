using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existsProduct = productCollection.Find(p => true).Any();

            if(!existsProduct)
            {
                productCollection.InsertManyAsync(GetMyProducts());
            }
        }

        public static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {

            };
        }
    }
}