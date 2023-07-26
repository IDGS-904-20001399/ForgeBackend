using Forge.Models;
using Microsoft.EntityFrameworkCore;

namespace Forge.Persistence.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}