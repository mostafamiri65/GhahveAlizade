using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class BannerConfiguration : BaseEntityConfiguration<Banner>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Banner> builder)
	{
		builder.ToTable("Banners");

		builder.Property(x => x.Title).IsRequired().HasMaxLength(300);
		builder.Property(x => x.Subtitle).HasMaxLength(500);
		builder.Property(x => x.Description).HasMaxLength(2000);
		builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);
		builder.Property(x => x.MobileImageUrl).HasMaxLength(1000);
		builder.Property(x => x.AltText).HasMaxLength(500);
		builder.Property(x => x.LinkUrl).HasMaxLength(1000);
		builder.Property(x => x.ButtonText).HasMaxLength(100);
		builder.Property(x => x.Target).HasMaxLength(20);

		builder.HasIndex(x => new { x.IsActive, x.StartsAtUtc, x.ExpiresAtUtc })
			.HasDatabaseName("IX_Banners_Active_Date");
	}
}