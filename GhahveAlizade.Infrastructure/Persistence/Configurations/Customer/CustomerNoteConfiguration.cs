using GhahveAlizade.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Customer;

public class CustomerNoteConfiguration : BaseEntityConfiguration<CustomerNote>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CustomerNote> builder)
	{
		builder.ToTable("CustomerNotes");

		builder.Property(x => x.Content)
			.IsRequired()
			.HasMaxLength(4000);

		builder.Property(x => x.Category)
			.HasMaxLength(100);

		builder.HasIndex(x => new { x.CustomerId, x.CreatedAtUtc })
			.HasDatabaseName("IX_CustomerNotes_Customer_Date");

		builder.HasOne(x => x.Customer)
			.WithMany(c => c.Notes)
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}