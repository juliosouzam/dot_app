using core.Data;
using core.DTOs.Stocks;
using core.DTOs.Stocks.Filters;
using core.Interfaces;
using core.Models;
using Microsoft.EntityFrameworkCore;

namespace core.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Stock> CreateAsync(Stock model)
        {
            await _context.Stocks.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var model = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (model == null)
            {
                return null;
            }
            _context.Stocks.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<List<Stock>> GetAllAsync(QueryStockObject query)
        {
            var stocks = _context.Stocks.Include(stock => stock.Comments).ThenInclude(c => c.User).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(stock => stock.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(stock => stock.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(stock => stock.Symbol) : stocks.OrderBy(stock => stock.Symbol);
                }
            }

            var skipNumber = (query.Page - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(stock => stock.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(stock => stock.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.Include(stock => stock.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(stock => stock.Symbol == symbol);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequest dto)
        {
            var model = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (model == null)
            {
                return null;
            }
            model.Symbol = dto.Symbol;
            model.CompanyName = dto.CompanyName;
            model.Purchase = dto.Purchase;
            model.LastDiv = dto.LastDiv;
            model.Industry = dto.Industry;
            model.MarketCap = dto.MarketCap;
            await _context.SaveChangesAsync();

            return model;
        }
    }
}