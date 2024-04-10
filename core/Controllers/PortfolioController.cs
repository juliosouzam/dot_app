using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using core.Extensions;
using core.Interfaces;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace core.Controllers
{
    [Route("api/portfolios")]
    [ApiController]
    public class PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        private readonly IStockRepository _stockRepository = stockRepository;

        private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userName = User.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return BadRequest("User not found");
            var portfolios = await _portfolioRepository.GetUserPortfolios(user);

            return Ok(portfolios);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Store(string symbol)
        {
            var userName = User.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (user == null)
                return BadRequest("User not found");
            if (stock == null)
                return BadRequest("Stock not found");
            var userPortfolio = await _portfolioRepository.GetUserPortfolios(user);
            if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock at portfolio");
            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                UserId = user.Id
            };
            await _portfolioRepository.CreateAsync(portfolio);

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var userName = User.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepository.GetUserPortfolios(user);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
            if (filteredStock.Count == 1)
            {
                await _portfolioRepository.DeleteAsync(user, symbol);
            }

            return NoContent();

        }
    }
}