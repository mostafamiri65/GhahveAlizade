using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.ToTable("Users");

		builder.Property(x => x.FirstName).HasMaxLength(200);
		builder.Property(x => x.LastName).HasMaxLength(200);
		builder.Property(x => x.AvatarUrl).HasMaxLength(1000);
		builder.Property(x => x.NationalCode).HasMaxLength(20);
		builder.Property(x => x.Gender).HasMaxLength(20);
		builder.Property(x => x.CreatedBy).HasMaxLength(450);
		builder.Property(x => x.UpdatedBy).HasMaxLength(450);

		builder.Property(x => x.IsPhoneVerified).HasDefaultValue(false);
		builder.Property(x => x.HasPassword).HasDefaultValue(false);
		builder.Property(x => x.LastLoginProvider).HasMaxLength(50);
		builder.Property(x => x.PhoneVerifiedAtUtc);

		builder.HasIndex(x => x.PhoneNumber)
			.HasFilter("[PhoneNumber] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("IX_Users_PhoneNumber");

		builder.HasIndex(x => x.NationalCode)
			.HasFilter("[NationalCode] IS NOT NULL")
			.HasDatabaseName("IX_Users_NationalCode");

		builder.HasIndex(x => x.IsActive)
			.HasDatabaseName("IX_Users_IsActive");
	}
}