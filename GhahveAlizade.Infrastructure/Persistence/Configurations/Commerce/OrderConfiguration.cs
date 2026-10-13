using GhahveAlizade.Domain.Commerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Commerce;

public class OrderConfiguration : BaseEntityConfiguration<Order>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Order> builder)
	{
		builder.ToTable("Orders");

		builder.Property(x => x.OrderNumber)
			.IsRequired()
			.HasMaxLength(50);


		builder.Property(x => x.CouponCode)
			.HasMaxLength(100);

		builder.Property(x => x.Currency)
			.IsRequired()
			.HasMaxLength(10)
			.HasDefaultValue("IRT");

		builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
		builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.ShippingAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.Total).HasColumnType("decimal(18,2)");

		builder.Property(x => x.CustomerNotes).HasMaxLength(2000);
		builder.Property(x => x.AdminNotes).HasMaxLength(2000);

		builder.HasIndex(x => x.OrderNumber)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Orders_OrderNumber");

		builder.HasIndex(x => new { x.CustomerId, x.CreatedAtUtc })
			.HasDatabaseName("IX_Orders_Customer_Date");

		builder.HasIndex(x => new { x.Status, x.CreatedAtUtc })
			.HasDatabaseName("IX_Orders_Status_Date");

		builder.HasIndex(x => x.UserId)
			.HasDatabaseName("IX_Orders_UserId");

		// ============ Relations ============
		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);

		// ⬅️ این‌جا "WithOne" صریح می‌ذاریم که Navigationها رو به هم وصل کنه
		builder.HasMany(o => o.Payments)
			.WithOne(p => p.Order)
			.HasForeignKey(p => p.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(o => o.Shipments)
			.WithOne(s => s.Order)
			.HasForeignKey(s => s.OrderId)
			.OnDelete(DeleteBehavior.Restrict);

		// Items
		builder.HasMany(o => o.Items)
			.WithOne(i => i.Order)
			.HasForeignKey(i => i.OrderId)
			.OnDelete(DeleteBehavior.Cascade);

		// Addresses
		builder.HasMany(o => o.Addresses)
			.WithOne(a => a.Order)
			.HasForeignKey(a => a.OrderId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}