using GhahveAlizade.Domain.Wishlist;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Wishlist;

public class WishlistConfiguration : BaseEntityConfiguration<Domain.Wishlist.Wishlist>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Domain.Wishlist.Wishlist> builder)
	{
		builder.ToTable("Wishlists");

		builder.Property(x => x.UserId).HasMaxLength(450);
		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
		builder.Property(x => x.ShareToken).HasMaxLength(100);

		builder.HasIndex(x => x.UserId)
			.HasDatabaseName("IX_Wishlists_UserId");

		builder.HasIndex(x => x.ShareToken)
			.HasFilter("[ShareToken] IS NOT NULL")
			.HasDatabaseName("IX_Wishlists_ShareToken");

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}