using GhahveAlizade.Domain.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Loyalty;

public class CustomerPointAccountConfiguration : BaseEntityConfiguration<CustomerPointAccount>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CustomerPointAccount> builder)
	{
		builder.ToTable("CustomerPointAccounts");

		builder.Property(x => x.Tier).HasMaxLength(50);

		builder.HasIndex(x => x.CustomerId)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CustomerPointAccounts_CustomerId");

		builder.HasIndex(x => new { x.Tier, x.CurrentPoints })
			.HasDatabaseName("IX_CustomerPointAccounts_Tier_Points");

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}