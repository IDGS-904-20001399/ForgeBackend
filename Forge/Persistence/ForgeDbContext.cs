using Forge.Models;
using Forge.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Forge.Persistence
{
    public class ForgeDbContext : DbContext
    {
        public ForgeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products {get; set;} = null;

        protected override void OnModelCreating(ModelBuilder builder){
            builder.ApplyConfigurationsFromAssembly(typeof(ForgeDbContext).Assembly);
        }
    }
}