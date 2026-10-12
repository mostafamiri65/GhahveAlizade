using GhahveAlizade.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Accounting;

public class AccountingSyncLogConfiguration : BaseEntityConfiguration<AccountingSyncLog>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AccountingSyncLog> builder)
	{
		builder.ToTable("AccountingSyncLogs");

		builder.Property(x => x.EntityType).IsRequired().HasMaxLength(100);
		builder.Property(x => x.Direction).IsRequired().HasMaxLength(20);
		builder.Property(x => x.ExternalSystem).HasMaxLength(100);
		builder.Property(x => x.RequestPayload).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ResponsePayload).HasColumnType("nvarchar(max)");
		builder.Property(x => x.ErrorMessage).HasMaxLength(4000);

		builder.HasIndex(x => new { x.EntityType, x.EntityId })
			.HasDatabaseName("IX_AccountingSyncLogs_Entity");

		builder.HasIndex(x => new { x.IsSuccessful, x.SyncedAtUtc })
			.HasDatabaseName("IX_AccountingSyncLogs_Success_Date");
	}
}