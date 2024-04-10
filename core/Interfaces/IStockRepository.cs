using core.DTOs.Stocks;
using core.DTOs.Stocks.Filters;
using core.Models;

namespace core.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryStockObject query);

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);

        Task<Stock> CreateAsync(Stock model);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequest dto);

        Task<Stock?> DeleteAsync(int id);
    }
}