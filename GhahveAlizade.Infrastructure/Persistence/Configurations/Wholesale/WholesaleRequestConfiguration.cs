using GhahveAlizade.Domain.Wholesale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Wholesale;

public class WholesaleRequestConfiguration : BaseEntityConfiguration<WholesaleRequest>
{
	protected override void ConfigureEntity(EntityTypeBuilder<WholesaleRequest> builder)
	{
		builder.ToTable("WholesaleRequests");

		builder.Property(x => x.CompanyName).IsRequired().HasMaxLength(300);
		builder.Property(x => x.BusinessType).HasMaxLength(100);
		builder.Property(x => x.EconomicCode).HasMaxLength(50);
		builder.Property(x => x.RegistrationNumber).HasMaxLength(50);
		builder.Property(x => x.ContactPerson).IsRequired().HasMaxLength(200);
		builder.Property(x => x.ContactPhone).IsRequired().HasMaxLength(20);
		builder.Property(x => x.ContactEmail).HasMaxLength(256);
		builder.Property(x => x.City).HasMaxLength(100);
		builder.Property(x => x.Province).HasMaxLength(100);
		builder.Property(x => x.Address).HasMaxLength(1000);
		builder.Property(x => x.ProductInterests).HasMaxLength(2000);
		builder.Property(x => x.EstimatedMonthlyVolume).HasMaxLength(200);
		builder.Property(x => x.Message).HasMaxLength(4000);
		builder.Property(x => x.ReviewedBy).HasMaxLength(450);
		builder.Property(x => x.ReviewNotes).HasMaxLength(2000);

		builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
			.HasDatabaseName("IX_WholesaleRequests_Status_Date");

		builder.HasIndex(x => x.ContactPhone)
			.HasDatabaseName("IX_WholesaleRequests_ContactPhone");
	}
}