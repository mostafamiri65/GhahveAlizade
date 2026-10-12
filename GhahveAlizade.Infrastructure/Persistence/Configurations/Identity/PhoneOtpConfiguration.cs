using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class PhoneOtpConfiguration : BaseEntityConfiguration<PhoneOtp>
{
	protected override void ConfigureEntity(EntityTypeBuilder<PhoneOtp> builder)
	{
		builder.ToTable("PhoneOtps");

		builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
		builder.Property(x => x.Code).IsRequired().HasMaxLength(10);
		builder.Property(x => x.Purpose).HasMaxLength(50);
		builder.Property(x => x.IpAddress).HasMaxLength(50);
		builder.Property(x => x.UserAgent).HasMaxLength(500);

		// ایندکس برای جستجوی سریع
		builder.HasIndex(x => new { x.PhoneNumber, x.IsUsed, x.ExpiresAtUtc })
			.HasDatabaseName("IX_PhoneOtps_Phone_Status");

		builder.HasIndex(x => x.ExpiresAtUtc)
			.HasDatabaseName("IX_PhoneOtps_Expiry");
	}
}