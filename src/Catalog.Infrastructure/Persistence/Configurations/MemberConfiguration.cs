using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Member> builder)
        {
            builder.Property(p => p.Name)
                   .HasColumnType(string.Format(VARCHAR_LIMIT_COLUMN_TYPE, "150"))
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Email)
                   .HasColumnType(string.Format(VARCHAR_LIMIT_COLUMN_TYPE, "150"))
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(p => p.Email).IsUnique();

            builder.Property(p => p.Password)
                   .HasColumnType(string.Format(VARCHAR_LIMIT_COLUMN_TYPE, "50"))
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Active)
                  .HasColumnType(BIT_COLUMN_TYPE)
                  .IsRequired();

            builder.HasMany(p => p.Games)
                   .WithMany(g => g.Members)
                   .UsingEntity<Dictionary<string, object>>(
                        "tbMembertbGame",
                        j => j.HasOne<Game>()
                              .WithMany()
                              .HasForeignKey("IdGame")
                              .HasConstraintName("FK_tbMembertbGame_Game")
                              .OnDelete(DeleteBehavior.Cascade),

                        j => j.HasOne<Member>()
                              .WithMany()
                              .HasForeignKey("IdMember")
                              .HasConstraintName("FK_tbMembertbGame_Member")
                              .OnDelete(DeleteBehavior.Cascade),

                        j =>
                        {
                            j.HasKey("IdMember", "IdGame");
                            j.Property<int>("IdMember");
                            j.Property<int>("IdGame");
                        });
        }
    }
}
