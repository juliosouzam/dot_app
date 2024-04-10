using core.DTOs.Stocks;
using core.DTOs.Stocks.Filters;
using core.Interfaces;
using core.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace core.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        private readonly IStockRepository _stockRepository = stockRepository;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery] QueryStockObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(queryObject);

            var stockDTO = stocks.Select(stock => stock.ToStockDTO()).ToList();

            return Ok(stockDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Show([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] CreateStockRequest body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = body.ToStockFromCreateDTO();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(Show), new
            {
                id = stockModel.Id
            }, stockModel.ToStockDTO());
        }

        [HttpPut("{id:int}")]
        // [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.UpdateAsync(id, body);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete("{id:int}")]
        // [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}