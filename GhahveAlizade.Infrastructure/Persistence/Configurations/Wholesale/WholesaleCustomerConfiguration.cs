using GhahveAlizade.Domain.Wholesale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Wholesale;

public class WholesaleCustomerConfiguration : BaseEntityConfiguration<WholesaleCustomer>
{
	protected override void ConfigureEntity(EntityTypeBuilder<WholesaleCustomer> builder)
	{
		builder.ToTable("WholesaleCustomers");

		builder.Property(x => x.CompanyName).HasMaxLength(300);
		builder.Property(x => x.BusinessType).HasMaxLength(100);
		builder.Property(x => x.EconomicCode).HasMaxLength(50);
		builder.Property(x => x.RegistrationNumber).HasMaxLength(50);
		builder.Property(x => x.ContactPerson).HasMaxLength(200);
		builder.Property(x => x.ContactPhone).HasMaxLength(20);
		builder.Property(x => x.ContactEmail).HasMaxLength(256);
		builder.Property(x => x.CreditLimit).HasColumnType("decimal(18,2)");
		builder.Property(x => x.ApprovedBy).HasMaxLength(450);
		builder.Property(x => x.Notes).HasMaxLength(2000);

		builder.HasIndex(x => x.CustomerId)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_WholesaleCustomers_CustomerId");

		builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
			.HasDatabaseName("IX_WholesaleCustomers_Status_Date");

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}