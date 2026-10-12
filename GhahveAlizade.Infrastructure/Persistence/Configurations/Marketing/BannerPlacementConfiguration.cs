using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class BannerPlacementConfiguration : BaseEntityConfiguration<BannerPlacement>
{
	protected override void ConfigureEntity(EntityTypeBuilder<BannerPlacement> builder)
	{
		builder.ToTable("BannerPlacements");

		builder.HasIndex(x => new { x.BannerId, x.Placement })
			.HasDatabaseName("IX_BannerPlacements_Banner_Placement");

		builder.HasIndex(x => new { x.Placement, x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_BannerPlacements_Placement_Active_Order");

		builder.HasOne(x => x.Banner)
			.WithMany(b => b.Placements)
			.HasForeignKey(x => x.BannerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}