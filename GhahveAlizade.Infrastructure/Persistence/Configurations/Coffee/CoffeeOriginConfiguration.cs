using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Coffee;

public class CoffeeOriginConfiguration : BaseEntityConfiguration<CoffeeOrigin>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CoffeeOrigin> builder)
	{
		builder.ToTable("CoffeeOrigins");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.CountryCode)
			.HasMaxLength(5);

		builder.Property(x => x.FlagUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_CoffeeOrigins_Slug");

		builder.HasIndex(x => new { x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_CoffeeOrigins_Active_Order");
	}
}