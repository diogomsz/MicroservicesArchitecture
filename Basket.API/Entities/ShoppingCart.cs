namespace Basket.API.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    /// <summary>
    ///     Construtor padrão
    /// </summary>
    public ShoppingCart() { }

    /// <summary>
    ///     Construtor que recebe como parâmetro o nome do usuário
    /// </summary>
    /// <param name="userName">
    ///     Nome do usuário
    /// </param>
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    /// <summary>
    ///     Calcula o preço total da cesta
    /// </summary>
    public decimal TotalPrice
    {
        get
        {
            decimal totalprice = 0;
            foreach (var item in Items)
            {
                totalprice += item.Price * item.Quantity;
            }

            return totalprice;
        }
    }
}
