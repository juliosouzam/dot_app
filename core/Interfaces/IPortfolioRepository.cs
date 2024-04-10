using core.Models;

namespace core.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolios(AppUser user);

        Task<Portfolio> CreateAsync(Portfolio portfolio);

        Task<Portfolio?> DeleteAsync(AppUser user, string symbol);
    }
}