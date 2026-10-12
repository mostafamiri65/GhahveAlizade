using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class CampaignConfiguration : BaseEntityConfiguration<Campaign>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Campaign> builder)
	{
		builder.ToTable("Campaigns");

		builder.Property(x => x.Name).IsRequired().HasMaxLength(300);
		builder.Property(x => x.Description).HasMaxLength(2000);
		builder.Property(x => x.Code).HasMaxLength(50);
		builder.Property(x => x.CampaignType).IsRequired().HasMaxLength(50);
		builder.Property(x => x.TargetAudienceJson).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ContentJson).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("[Code] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("UX_Campaigns_Code");

		builder.HasIndex(x => new { x.IsActive, x.StartsAtUtc, x.EndsAtUtc })
			.HasDatabaseName("IX_Campaigns_Active_Date");
	}
}