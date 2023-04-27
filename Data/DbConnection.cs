using Microsoft.EntityFrameworkCore;
using ShoppingOnline.Models.Entity;

namespace ShoppingOnline.Data
{
    public class DbConnection: DbContext
    {
        public DbConnection(DbContextOptions options):base(options) { }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
