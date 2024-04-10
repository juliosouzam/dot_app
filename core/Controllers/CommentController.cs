using core.DTOs.Comments;
using core.Extensions;
using core.Interfaces;
using core.Mappers;
using core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace core.Controllers
{
    [Route("api/comments")]
    public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager) : Controller
    {
        private readonly ICommentRepository _commentRepository = commentRepository;

        private readonly IStockRepository _stockRepository = stockRepository;

        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepository.GetAllAsync();
            var commentsDTO = comments.Select(comment => comment.ToCommentDTO());

            return Ok(commentsDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Show([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Store([FromRoute] int stockId, [FromBody] CreateCommentDTO body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock == null)
            {
                return BadRequest("Stock does not exists");
            }
            var userName = User.GetUserName();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var comment = body.ToCommentFromCreateDTO(stockId);
            comment.UserId = user.Id;
            await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(Show), new { id = comment.Id }, comment.ToCommentDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequest body)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.UpdateAsync(id, body);
            if (comment == null)
            {
                return NotFound("Comment does not exists");
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound("Comment does not exists");
            }

            return NoContent();
        }
    }
}