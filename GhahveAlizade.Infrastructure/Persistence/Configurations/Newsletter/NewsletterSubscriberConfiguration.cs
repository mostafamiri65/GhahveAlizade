using GhahveAlizade.Domain.Newsletter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Newsletter;

public class NewsletterSubscriberConfiguration : BaseEntityConfiguration<NewsletterSubscriber>
{
	protected override void ConfigureEntity(EntityTypeBuilder<NewsletterSubscriber> builder)
	{
		builder.ToTable("NewsletterSubscribers");

		builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
		builder.Property(x => x.FirstName).HasMaxLength(200);
		builder.Property(x => x.LastName).HasMaxLength(200);
		builder.Property(x => x.PhoneNumber).HasMaxLength(20);
		builder.Property(x => x.Source).HasMaxLength(100);
		builder.Property(x => x.Tags).HasMaxLength(1000);
		builder.Property(x => x.VerificationToken).HasMaxLength(200);
		builder.Property(x => x.UnsubscribeReason).HasMaxLength(1000);

		builder.HasIndex(x => x.Email)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_NewsletterSubscribers_Email");

		builder.HasIndex(x => new { x.IsActive, x.IsVerified })
			.HasDatabaseName("IX_NewsletterSubscribers_Status");
	}
}