using core.DTOs.Stocks;
using core.Models;

namespace core.Mappers
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock model)
        {
            return new StockDTO
            {
                Id = model.Id,
                Symbol = model.Symbol,
                CompanyName = model.CompanyName,
                Purchase = model.Purchase,
                LastDiv = model.LastDiv,
                Industry = model.Industry,
                MarketCap = model.MarketCap,
                Comments = model.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequest dto)
        {
            return new Stock
            {
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDiv = dto.LastDiv,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap
            };
        }
    }
}