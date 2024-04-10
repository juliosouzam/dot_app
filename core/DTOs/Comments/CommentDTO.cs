namespace core.DTOs.Comments
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; } = string.Empty;

        public int? StockId { get; set; }
    }
}