using GhahveAlizade.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Review;

public class ReviewReplyConfiguration : BaseEntityConfiguration<ReviewReply>
{
	protected override void ConfigureEntity(EntityTypeBuilder<ReviewReply> builder)
	{
		builder.ToTable("ReviewReplies");

		builder.Property(x => x.Content).IsRequired().HasMaxLength(4000);
		builder.Property(x => x.RepliedByUserId).HasMaxLength(450);
		builder.Property(x => x.RepliedByName).HasMaxLength(200);

		builder.HasIndex(x => new { x.ProductReviewId, x.CreatedAtUtc })
			.HasDatabaseName("IX_ReviewReplies_Review_Date");

		builder.HasOne(x => x.ProductReview)
			.WithMany(r => r.Replies)
			.HasForeignKey(x => x.ProductReviewId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}