using GhahveAlizade.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Inventory;

public class WarehouseConfiguration : BaseEntityConfiguration<Warehouse>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Warehouse> builder)
	{
		builder.ToTable("Warehouses");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Code)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Address)
			.HasMaxLength(1000);

		builder.Property(x => x.City)
			.HasMaxLength(100);

		builder.Property(x => x.Province)
			.HasMaxLength(100);

		builder.Property(x => x.PostalCode)
			.HasMaxLength(20);

		builder.Property(x => x.Phone)
			.HasMaxLength(20);

		builder.Property(x => x.ManagerName)
			.HasMaxLength(200);

		builder.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Warehouses_Code");

		builder.HasIndex(x => new { x.IsActive, x.IsDefault })
			.HasDatabaseName("IX_Warehouses_Active");
	}
}