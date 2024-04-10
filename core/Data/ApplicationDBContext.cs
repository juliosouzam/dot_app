using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace core.Data
{
    public class ApplicationDBContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.UserId, p.StockId }));
            builder.Entity<Portfolio>().HasOne(u => u.User).WithMany(u => u.Portfolios).HasForeignKey(p => p.UserId);
            builder.Entity<Portfolio>().HasOne(u => u.Stock).WithMany(u => u.Portfolios).HasForeignKey(p => p.StockId);

            List<IdentityRole> roles =
            [
                new() {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new() {
                    Name = "User",
                    NormalizedName = "USER"
                }
            ];

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}