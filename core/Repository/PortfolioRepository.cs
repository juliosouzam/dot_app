using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using core.Data;
using core.Interfaces;
using core.Models;
using Microsoft.EntityFrameworkCore;

namespace core.Repository
{
    public class PortfolioRepository(ApplicationDBContext context) : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser user, string symbol)
        {
            var model = await _context.Portfolios.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (model == null) return null;

            _context.Portfolios.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<List<Stock>> GetUserPortfolios(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.UserId == user.Id)
                .Select(portfolio => new Stock
                {
                    Id = portfolio.StockId,
                    Symbol = portfolio.Stock.Symbol,
                    CompanyName = portfolio.Stock.CompanyName,
                    Purchase = portfolio.Stock.Purchase,
                    LastDiv = portfolio.Stock.LastDiv,
                    Industry = portfolio.Stock.Industry,
                    MarketCap = portfolio.Stock.MarketCap,
                }).ToListAsync();
        }
    }
}