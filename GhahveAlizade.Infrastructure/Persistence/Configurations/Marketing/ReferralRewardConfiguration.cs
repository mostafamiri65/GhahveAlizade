using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class ReferralRewardConfiguration : BaseEntityConfiguration<ReferralReward>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ReferralReward> builder)
	{
		builder.ToTable("ReferralRewards");

		builder.Property(x => x.RewardType).IsRequired().HasMaxLength(50);
		builder.Property(x => x.RewardValue).HasColumnType("decimal(18,2)");

		builder.HasIndex(x => new { x.CustomerId, x.IsGranted })
			.HasDatabaseName("IX_ReferralRewards_Customer_Granted");

		builder.HasOne(x => x.Referral)
			.WithMany(r => r.Rewards)
			.HasForeignKey(x => x.ReferralId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}