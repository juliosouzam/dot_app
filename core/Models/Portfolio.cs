using System.ComponentModel.DataAnnotations.Schema;

namespace core.Models
{
    [Table("portfolios")]
    public class Portfolio
    {
        public string UserId { get; set; } = string.Empty;

        public int StockId { get; set; }

        public AppUser User { get; set; }

        public Stock Stock { get; set; }
    }
}