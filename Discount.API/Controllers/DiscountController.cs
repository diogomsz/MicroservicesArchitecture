using Discount.API.Entities;
using Discount.API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;
    /// <summary>
    ///     Injeção de Dependência
    /// </summary>
    /// <param name="discountRepository">
    ///     Com este parâmetro podemos usar os métodos
    ///     para comunicarmos com o banco de dados usando padrão Repository
    /// </param>
    public DiscountController(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    /// <summary>
    ///     Obtendo um cupom de desconto
    /// </summary>
    /// <param name="name">
    ///     Nome do produto
    /// </param>
    /// <returns>
    ///     Retorna um cupom de desconto
    /// </returns>
    [HttpGet("{productName}", Name = "GetDiscount")]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName)
    {
        Coupon coupon = await _discountRepository.GetDiscount(productName);
        return Ok(coupon);
    }

    /// <summary>
    ///     Criando um cupom de desconto
    /// </summary>
    /// <param name="coupon">
    ///     Dados do cupom
    /// </param>
    /// <returns>
    ///     Retorna um cupom de desconto
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
    {
        await _discountRepository.CreateDiscount(coupon);
        return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
    }

    /// <summary>
    ///     Atualizando os dados do cupom
    /// </summary>
    /// <param name="coupon">
    ///     Dados do cupom
    /// </param>
    /// <returns>
    ///     Os dados do cupom atualizado
    /// </returns>
    [HttpPut]
    public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
    {
        return Ok(await _discountRepository.UpdateDiscount(coupon));
    }

    /// <summary>
    ///     Apagando um cupom de desconto
    /// </summary>
    /// <param name="productName">
    ///     Nome do produto
    /// </param>
    /// <returns>
    ///     Retorna true caso o cupom seja apagado com sucesso, 
    ///     retorna false caso não seja executado com sucesso
    /// </returns>
    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    public async Task<ActionResult<bool>> DeleteDiscount(string productName)
    {
        return Ok(await _discountRepository.DeleteDiscount(productName));
    }
}
