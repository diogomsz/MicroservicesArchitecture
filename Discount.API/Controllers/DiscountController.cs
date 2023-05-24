using Discount.API.Repository.Interfaces;

namespace Discount.API.Controllers;

public class DiscountController
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

    // PAREI AQUI //////////////////////////////////
}
