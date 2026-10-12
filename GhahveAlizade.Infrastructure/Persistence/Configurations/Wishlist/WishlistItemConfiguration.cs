using GhahveAlizade.Domain.Wishlist;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Wishlist;

public class WishlistItemConfiguration : BaseEntityConfiguration<WishlistItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<WishlistItem> builder)
	{
		builder.ToTable("WishlistItems");

		builder.Property(x => x.Notes).HasMaxLength(1000);

		builder.HasIndex(x => new { x.WishlistId, x.ProductId, x.ProductVariantId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_WishlistItems_Wishlist_Product_Variant");

		builder.HasOne(x => x.Wishlist)
			.WithMany(w => w.Items)
			.HasForeignKey(x => x.WishlistId)
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