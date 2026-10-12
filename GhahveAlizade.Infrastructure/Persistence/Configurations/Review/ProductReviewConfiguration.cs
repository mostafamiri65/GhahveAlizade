using GhahveAlizade.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Review;

public class ProductReviewConfiguration : BaseEntityConfiguration<ProductReview>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ProductReview> builder)
	{
		builder.ToTable("ProductReviews");

		builder.Property(x => x.AuthorName).HasMaxLength(200);
		builder.Property(x => x.AuthorEmail).HasMaxLength(256);
		builder.Property(x => x.Title).HasMaxLength(500);
		builder.Property(x => x.Content).HasMaxLength(4000);

		builder.HasIndex(x => new { x.ProductId, x.IsApproved, x.CreatedAtUtc })
			.HasDatabaseName("IX_ProductReviews_Product_Approved_Date");

		builder.HasIndex(x => new { x.CustomerId, x.ProductId })
			.HasDatabaseName("IX_ProductReviews_Customer_Product");

		builder.HasOne(x => x.Product)
			.WithMany(p => p.Reviews)
			.HasForeignKey(x => x.ProductId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Customer)
			.WithMany()
			.HasForeignKey(x => x.CustomerId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}