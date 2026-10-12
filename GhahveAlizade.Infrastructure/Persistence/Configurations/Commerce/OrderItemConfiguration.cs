using GhahveAlizade.Domain.Commerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Commerce;

public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<OrderItem> builder)
	{
		builder.ToTable("OrderItems");

		builder.Property(x => x.ProductName)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.ProductSku).HasMaxLength(100);
		builder.Property(x => x.ProductImageUrl).HasMaxLength(1000);
		builder.Property(x => x.VariantName).HasMaxLength(500);

		builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TaxAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

		builder.Property(x => x.Notes)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.OrderId)
			.HasDatabaseName("IX_OrderItems_OrderId");

		builder.HasIndex(x => x.ProductId)
			.HasDatabaseName("IX_OrderItems_ProductId");

		builder.HasOne(x => x.Order)
			.WithMany(o => o.Items)
			.HasForeignKey(x => x.OrderId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Product)
			.WithMany()
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.ProductVariant)
			.WithMany()
			.HasForeignKey(x => x.ProductVariantId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}