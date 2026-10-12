using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
	protected override void ConfigureEntity(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.ToTable("RefreshTokens");
		builder.Property(x => x.Token).IsRequired().HasMaxLength(500);
		builder.HasIndex(x => x.Token).IsUnique().HasFilter("[IsDeleted] = 0");
		builder.HasIndex(x => new { x.UserId, x.IsRevoked });
	}
}