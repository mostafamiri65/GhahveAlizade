using GhahveAlizade.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Customer;

public class CustomerConfiguration : BaseEntityConfiguration<Domain.Customer.Customer>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Domain.Customer.Customer> builder)
	{
		builder.ToTable("Customers");

		builder.Property(x => x.UserId)
			.HasMaxLength(450);

		builder.Property(x => x.FirstName)
			.HasMaxLength(200);

		builder.Property(x => x.LastName)
			.HasMaxLength(200);

		builder.Property(x => x.Email)
			.HasMaxLength(256);

		builder.Property(x => x.PhoneNumber)
			.HasMaxLength(20);

		builder.Property(x => x.NationalCode)
			.HasMaxLength(20);

		builder.Property(x => x.Gender)
			.HasMaxLength(20);

		builder.Property(x => x.AvatarUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.TotalSpent)
			.HasColumnType("decimal(18,2)");

		builder.HasIndex(x => x.UserId)
			.HasDatabaseName("IX_Customers_UserId");

		builder.HasIndex(x => x.Email)
			.HasFilter("[Email] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("IX_Customers_Email");

		builder.HasIndex(x => x.PhoneNumber)
			.HasFilter("[PhoneNumber] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("IX_Customers_Phone");

		builder.HasIndex(x => new { x.CustomerGroupId, x.IsActive })
			.HasDatabaseName("IX_Customers_Group_Active");

		builder.HasOne(x => x.CustomerGroup)
			.WithMany()
			.HasForeignKey(x => x.CustomerGroupId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}