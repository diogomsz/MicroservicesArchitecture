using Dapper;
using Discount.API.Entities;
using Discount.API.Repository.Interfaces;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    /// <summary>
    ///     Injeção de dependência
    /// </summary>
    /// <param name="configuration">
    ///     Instância de IConfiguration para obter a ConnectionString
    /// </param>
    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new
            ArgumentNullException(nameof(configuration));

    }

    /// <summary>
    ///     Obtendo a string de conexão com o PostgreSQL
    /// </summary>
    /// <returns>
    ///     Instância de NpgsqlConnection que representa a conexão
    /// </returns>
    private NpgsqlConnection GetConnectionPostgreSQL()
    {
        using var connection = new NpgsqlConnection(_configuration
            .GetValue<string>("DatabaseSettings:ConnectionString"));
        return connection;
    }

    /// <summary>
    ///     Obtem um cupom de desconto pelo nome do produto
    /// </summary>
    /// <param name="productName">
    ///     Nome do produto
    /// </param>
    /// <returns>
    ///     Retorna um cupom de desconto
    /// </returns>
    public async Task<Coupon> GetDiscount(string productName)
    {
        NpgsqlConnection connection = GetConnectionPostgreSQL();

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if (coupon == null)
        {
            return new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
        }
        return coupon;
    }

    /// <summary>
    ///     Cria um cupom de desconto
    /// </summary>
    /// <param name="coupon">
    ///     Dados do desconto
    /// </param>
    /// <returns>
    ///     Retorna true se foi criado com sucesso,
    ///     false se houve alguma falha
    /// </returns>
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        NpgsqlConnection connection = GetConnectionPostgreSQL();
        var affected = await connection.ExecuteAsync
            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES " +
            "(@ProductName, @Description, @Amount)", new
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            });
        if (affected == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    ///     Atualiza os dados de um cupom de desconto
    /// </summary>
    /// <param name="coupon">
    ///     Dadoa atualizado de um cupom de desconto
    /// </param>
    /// <returns>
    ///     Retorna true se foi atualizado com sucesso,
    ///     false se houve alguma falha
    /// </returns>
    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        NpgsqlConnection connection = GetConnectionPostgreSQL();

        var affected = await connection.ExecuteAsync
            ("UPDATE Coupon SET " +
            "(ProductName = @ProductName, Description = @Description, Amount = @Amount) " +
            "WHERE Id = @Id ", new
            {
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
                Id = coupon.Id,
            });
        if (affected == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    ///     Apaga um cupom de desconto
    /// </summary>
    /// <param name="productName">
    ///     Nome do produto
    /// </param>
    /// <returns>
    ///     Retorna true se foi deletado com sucesso,
    ///     false se houve alguma falha
    /// </returns>
    public async Task<bool> DeleteDiscount(string productName)
    {
        NpgsqlConnection connection = GetConnectionPostgreSQL();

        var affected = await connection.ExecuteAsync
            ("DELETR FROM Coupon WHERE ProductName = @ProductName", new
            {
                ProductName = productName
            });
        if (affected == 0)
        {
            return false;
        }
        return true;
    }
}
