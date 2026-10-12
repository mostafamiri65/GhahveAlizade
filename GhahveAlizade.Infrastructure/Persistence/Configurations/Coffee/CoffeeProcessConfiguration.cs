using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class CoffeeProcessConfiguration : BaseEntityConfiguration<CoffeeProcess>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CoffeeProcess> builder)
	{
		builder.ToTable("CoffeeProcesses");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.IconUrl)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CoffeeProcesses_Slug");

		builder.HasIndex(x => new { x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_CoffeeProcesses_Active_Order");
	}
}