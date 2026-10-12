using GhahveAlizade.Domain.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Marketing;

public class AbandonedCartNotificationConfiguration : BaseEntityConfiguration<AbandonedCartNotification>
{
	protected override void ConfigureEntity(EntityTypeBuilder<AbandonedCartNotification> builder)
	{
		builder.ToTable("AbandonedCartNotifications");

		builder.Property(x => x.Channel).IsRequired().HasMaxLength(50);
		builder.Property(x => x.Subject).HasMaxLength(500);
		builder.Property(x => x.Content).HasColumnType("nvarchar(max)");

		builder.HasIndex(x => new { x.AbandonedCartId, x.Channel })
			.HasDatabaseName("IX_AbandonedCartNotifications_Cart_Channel");

		builder.HasOne(x => x.AbandonedCart)
			.WithMany(c => c.Notifications)
			.HasForeignKey(x => x.AbandonedCartId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}