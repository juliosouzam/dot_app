using core.DTOs.Comments;
using core.Models;

namespace core.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment model)
        {
            return new CommentDTO
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.User.UserName,
                StockId = model.StockId
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentDTO dto, int stockId)
        {
            return new Comment
            {
                Title = dto.Title,
                Content = dto.Content,
                StockId = stockId
            };
        }
    }
}