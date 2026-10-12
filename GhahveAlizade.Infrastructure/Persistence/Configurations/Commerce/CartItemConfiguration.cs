using GhahveAlizade.Domain.Commerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Commerce;

public class CartItemConfiguration : BaseEntityConfiguration<CartItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<CartItem> builder)
	{
		builder.ToTable("CartItems");

		builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
		builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");

		builder.Property(x => x.Notes)
			.HasMaxLength(1000);

		builder.HasIndex(x => x.CartId)
			.HasDatabaseName("IX_CartItems_CartId");

		builder.HasIndex(x => new { x.ProductId, x.ProductVariantId })
			.HasDatabaseName("IX_CartItems_Product_Variant");

		builder.HasOne(x => x.Cart)
			.WithMany(c => c.Items)
			.HasForeignKey(x => x.CartId)
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