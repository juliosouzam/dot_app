using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace core.Models
{
    [Table("users")]
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = [];
    }
}