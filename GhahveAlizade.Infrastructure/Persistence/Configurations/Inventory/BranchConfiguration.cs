using GhahveAlizade.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Inventory;

public class BranchConfiguration : BaseEntityConfiguration<Branch>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Branch> builder)
	{
		builder.ToTable("Branches");

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

		builder.Property(x => x.Latitude)
			.HasColumnType("decimal(10,7)");

		builder.Property(x => x.Longitude)
			.HasColumnType("decimal(10,7)");

		builder.Property(x => x.WorkingHours)
			.HasMaxLength(500);

		builder.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Branches_Code");

		builder.HasIndex(x => new { x.IsActive, x.DisplayOrder })
			.HasDatabaseName("IX_Branches_Active_Order");
	}
}