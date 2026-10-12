using GhahveAlizade.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Settings;

public class AppSettingConfiguration : BaseEntityConfiguration<AppSetting>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AppSetting> builder)
	{
		builder.ToTable("AppSettings");

		builder.Property(x => x.Key).IsRequired().HasMaxLength(200);
		builder.Property(x => x.Value).HasColumnType("nvarchar(max)");
		builder.Property(x => x.Description).HasMaxLength(500);
		builder.Property(x => x.Group).HasMaxLength(100);
		builder.Property(x => x.ValueType).IsRequired().HasMaxLength(50);

		builder.HasIndex(x => x.Key)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_AppSettings_Key");

		builder.HasIndex(x => new { x.Group, x.DisplayOrder })
			.HasDatabaseName("IX_AppSettings_Group_Order");
	}
}