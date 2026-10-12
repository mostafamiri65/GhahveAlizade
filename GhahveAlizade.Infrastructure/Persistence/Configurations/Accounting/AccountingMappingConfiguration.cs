using GhahveAlizade.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Accounting;

public class AccountingMappingConfiguration : BaseEntityConfiguration<AccountingMapping>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AccountingMapping> builder)
	{
		builder.ToTable("AccountingMappings");

		builder.Property(x => x.LocalEntityType).IsRequired().HasMaxLength(100);
		builder.Property(x => x.ExternalEntityType).IsRequired().HasMaxLength(100);
		builder.Property(x => x.ExternalEntityId).IsRequired().HasMaxLength(200);
		builder.Property(x => x.ExternalSystem).HasMaxLength(100);

		builder.HasIndex(x => new { x.LocalEntityType, x.LocalEntityId, x.ExternalSystem })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_AccountingMappings_Local_External");

		builder.HasIndex(x => new { x.ExternalSystem, x.ExternalEntityId })
			.HasDatabaseName("IX_AccountingMappings_External");
	}
}