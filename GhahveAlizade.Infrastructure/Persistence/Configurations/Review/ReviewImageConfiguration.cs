using GhahveAlizade.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Review;

public class ReviewImageConfiguration : BaseEntityConfiguration<ReviewImage>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ReviewImage> builder)
	{
		builder.ToTable("ReviewImages");

		builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);
		builder.Property(x => x.AltText).HasMaxLength(500);

		builder.HasIndex(x => new { x.ProductReviewId, x.DisplayOrder })
			.HasDatabaseName("IX_ReviewImages_Review_Order");

		builder.HasOne(x => x.ProductReview)
			.WithMany(r => r.Images)
			.HasForeignKey(x => x.ProductReviewId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}