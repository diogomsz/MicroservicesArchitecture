using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    /// <summary>
    ///     Injeção de Dependência
    /// </summary>
    /// <param name="redisCache">
    ///     Parâmetro que representa o Cache Redis
    /// </param>
    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
    }

    /// <summary>
    ///     Obtendo uma cesta
    /// </summary>
    /// <param name="userName">
    ///     Nome do usuário
    /// </param>
    /// <returns>
    ///     Retorna um carrinho de compras
    /// </returns>
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await _redisCache.GetStringAsync(userName);

        if(string.IsNullOrEmpty(basket))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    /// <summary>
    ///     Atualizando uma cesta
    /// </summary>
    /// <param name="basket">
    ///     Cesta atualizada
    /// </param>
    /// <returns>
    ///     Retorna um carrinho de compras
    /// </returns>
    public async Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
        return await GetBasket(basket.UserName);
    }

    /// <summary>
    ///     Apaga uma cesta
    /// </summary>
    /// <param name="userName">
    ///     Nome do usuário
    /// </param>
    public async Task DeleteBasket(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }

}
