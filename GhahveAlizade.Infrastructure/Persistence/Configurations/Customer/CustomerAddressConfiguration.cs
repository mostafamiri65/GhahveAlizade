using GhahveAlizade.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Customer;

public class CustomerAddressConfiguration : BaseEntityConfiguration<CustomerAddress>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CustomerAddress> builder)
	{
		builder.ToTable("CustomerAddresses");

		builder.Property(x => x.Title)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.FirstName)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.LastName)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.PhoneNumber)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(x => x.Email)
			.HasMaxLength(256);

		builder.Property(x => x.Province)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.City)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.PostalCode)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(x => x.AddressLine1)
			.IsRequired()
			.HasMaxLength(1000);

		builder.Property(x => x.AddressLine2)
			.HasMaxLength(1000);

		builder.Property(x => x.Latitude)
			.HasColumnType("decimal(10,7)");

		builder.Property(x => x.Longitude)
			.HasColumnType("decimal(10,7)");

		builder.HasIndex(x => new { x.CustomerId, x.IsDefault })
			.HasDatabaseName("IX_CustomerAddresses_Customer_Default");

		builder.HasOne(x => x.Customer)
			.WithMany(c => c.Addresses)
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}