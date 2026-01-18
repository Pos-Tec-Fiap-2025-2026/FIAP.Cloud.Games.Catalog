using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations
{
    public class GameConfiguration : BaseConfiguration<Game>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Game> builder)
        {
            builder.Property(p => p.Name)
                    .HasColumnType(VARCHAR_MAX_COLUMN_TYPE)
                    .IsRequired();

            builder.Property(p => p.Description)
                    .HasColumnType(VARCHAR_MAX_COLUMN_TYPE)
                    .IsRequired();

            builder.Property(p => p.Price)
                    .HasColumnType(string.Format(DECIMAL_LIMIT_COLUMN_TYPE, 18,2))
                    .IsRequired();

            builder.Property(p => p.Active)
                    .HasColumnType(BIT_COLUMN_TYPE)
                    .IsRequired();
        }
    }
}
