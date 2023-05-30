using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Route("api/b1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;
    /// <summary>
    ///     Injeção de dependência
    /// </summary>  
    /// <param name="basketRepository">
    ///     Serviço do repositório
    /// </param>
    public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _discountGrpcService = discountGrpcService;
    }

    /// <summary>
    ///     Obtendo a cesta de um usuário
    /// </summary>
    /// <param name="userName">
    ///     Nome do usuário
    /// </param>
    /// <returns>
    ///     Retorna a cesta do usuário, caso seja o primeiro
    ///     acesso do mesmo, retorna uma nova cesta
    /// </returns>
    [HttpGet("{userName}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    /// <summary>
    ///     Atualizando a cesta
    /// </summary>
    /// <param name="basket">
    ///     Cesta do usuário
    /// </param>
    /// <returns>
    ///     Retorna a nova cesta do usuário
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        return Ok(await _basketRepository.UpdateBasket(basket));
    }

    /// <summary>
    ///     Apagando uma cesta
    /// </summary>
    /// <param name="userName">
    ///     Nome do usuário
    /// </param>
    /// <returns>
    ///     Retorna status 200 de deletado com sucesso
    /// </returns>
    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _basketRepository.DeleteBasket(userName);
        return Ok();
    }
}
