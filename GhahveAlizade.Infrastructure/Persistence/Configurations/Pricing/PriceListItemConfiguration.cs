using GhahveAlizade.Domain.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Pricing;

public class PriceListItemConfiguration : BaseEntityConfiguration<PriceListItem>
{
	protected override void ConfigureEntity(EntityTypeBuilder<PriceListItem> builder)
	{
		builder.ToTable("PriceListItems");

		builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
		builder.Property(x => x.CompareAtPrice).HasColumnType("decimal(18,2)");
		builder.Property(x => x.MinQuantity).HasColumnType("decimal(18,3)");

		builder.HasIndex(x => new { x.PriceListId, x.ProductId, x.ProductVariantId })
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_PriceListItems_List_Product_Variant");

		builder.HasOne(x => x.PriceList)
			.WithMany(p => p.Items)
			.HasForeignKey(x => x.PriceListId)
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