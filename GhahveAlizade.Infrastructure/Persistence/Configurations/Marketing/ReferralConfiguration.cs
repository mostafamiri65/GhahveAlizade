using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class ReferralConfiguration : BaseEntityConfiguration<Referral>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Referral> builder)
	{
		builder.ToTable("Referrals");

		builder.Property(x => x.ReferralCode).IsRequired().HasMaxLength(50);
		builder.Property(x => x.RefereeEmail).HasMaxLength(256);
		builder.Property(x => x.RefereePhone).HasMaxLength(20);
		builder.Property(x => x.Status).IsRequired().HasMaxLength(50);

		builder.HasIndex(x => x.ReferralCode)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Referrals_ReferralCode");

		builder.HasIndex(x => new { x.ReferrerCustomerId, x.Status })
			.HasDatabaseName("IX_Referrals_Referrer_Status");

		builder.HasOne(x => x.ReferrerCustomer)
			.WithMany()
			.HasForeignKey(x => x.ReferrerCustomerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.RefereeCustomer)
			.WithMany()
			.HasForeignKey(x => x.RefereeCustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}