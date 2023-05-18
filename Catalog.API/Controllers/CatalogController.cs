using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    /// <summary>
    ///     Injeção de Dependência para usarmos o repositório de Product
    ///     o escopo (AddScope) foi definido na classe Program
    /// </summary>
    /// <param name="productRepository">
    ///     Com este parâmetro podemos usar os métodos
    ///     para comunicarmos com o banco de dados usando padrão Repository
    /// </param>
    ///
    public CatalogController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Obtendo todos os produtos do repository
    /// </summary>
    /// <returns>
    ///     Status 200 com a lista dos produtos
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return Ok(products);
    }

    /// <summary>
    ///     Obtendo um produto pelo Id do mesmo
    /// </summary>
    /// <param name="id">
    ///     Representa o Id do Produto
    /// </param>
    /// <returns>
    ///     Retorna um status 200 caso encontre o Produto e
    ///     status 404 caso não encontre
    /// </returns>
    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _productRepository.GetProduct(id);
        if(product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    /// <summary>
    ///Obtendo um produto pela categoria
    /// </summary>
    /// <param name="category">
    ///     Categoria na qual queremos filtrar os produtos
    /// </param>
    /// <returns>
    ///     Retorna status 200 caso consiga filtrar por categoria e 
    ///     status 400 caso o valor do parâmetro seja null
    /// </returns>
    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(
        string category)
    {
        if(category is null)
        {
            return BadRequest("Invalid Category");
        }

        var products = await _productRepository.GetProductByCategory(category);
        return Ok(products);
    }

    /// <summary>
    ///     Cria (cadastra) um produto
    /// </summary>
    /// <param name="product">
    ///     Produto com todos os seus dados
    /// </param>
    /// <returns>
    ///     Retorna status 200 caso o produto seja criado com sucesso
    ///     e status 400 caso o produto passado no corpo da requisição
    ///     seja null
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if(product is null)
        {
            return BadRequest("Invalid product");
        }

        await _productRepository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    /// <summary>
    ///     Atualiza um produto
    /// </summary>
    /// <param name="product">
    ///     Produto com todos os seus dados
    /// </param>
    /// <returns>
    ///     Retorna status 200 caso o produto seja atualizado com sucesso
    ///     e status 400 caso o produto passado no corpo da requisição
    ///     seja null
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        if(product is null)
        {
            return BadRequest("Invalid Product");
        }
        return Ok(await _productRepository.UpdateProduct(product));
    }

    /// <summary>
    ///     Apaga um produto
    /// </summary>
    /// <param name="id">
    ///     Id do Produto
    /// </param>
    /// <returns>
    ///     Retorna status 200 caso o produto seja apagado com sucesso
    /// </returns>
    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        return Ok(await _productRepository.DeleteProduct(id));
    }
}
