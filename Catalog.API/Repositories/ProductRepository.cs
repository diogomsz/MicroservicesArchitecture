using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    /// <summary>
    ///     Obtendo uma lista de todos os produtos
    /// </summary>
    /// <returns>
    ///     Lista de Produtos
    /// </returns>
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catalogContext.Products.Find(p => true).ToListAsync();
    }

    /// <summary>
    ///     Obtendo um produto específico pelo Id
    /// </summary>
    /// <param name="id">
    ///     Id do produto
    /// </param>
    /// <returns>
    ///     Produto
    /// </returns>
    public async Task<Product> GetProduct(string id)
    {
        return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    ///     Obtendo uma lista de produtos de mesmo nome
    /// </summary>
    /// <param name="name">
    ///     Nome do produto específico
    /// </param>
    /// <returns>
    ///     Lista de produtos
    /// </returns>
    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter
            .ElemMatch(p => p.Name, name);
        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    /// <summary>
    ///     Obtendo uma lista de produtos de mesma categoria
    /// </summary>
    /// <param name="categoryName">
    ///     Nome da categoria
    /// </param>
    /// <returns>
    ///     Lista de produtos
    /// </returns>
    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter
            .Eq(p => p.Category, categoryName);
        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    /// <summary>
    ///     Cria/cadastra um produto
    /// </summary>
    /// <param name="product">
    ///     Instância da classe produto com os dados a serem cadastrados
    /// </param>
    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    /// <summary>
    ///     Atualiza um produto
    /// </summary>
    /// <param name="product">
    ///     Instância da classe produto com os dados a serem atualizados
    /// </param>
    /// <returns>
    ///     True se o produto foi atualizado com sucesso, false se não
    /// </returns>
    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _catalogContext.Products
            .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    /// <summary>
    ///     Deleta um produto
    /// </summary>
    /// <param name="id">
    ///     Id do produto
    /// </param>
    /// <returns>
    ///     True se o produto foi deletado com sucesso, false se não
    /// </returns>
    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter
            .Eq(p => p.Id, id);
        DeleteResult deleteResult = await _catalogContext.Products.DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
