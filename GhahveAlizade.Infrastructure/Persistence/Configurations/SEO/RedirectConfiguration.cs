using GhahveAlizade.Domain.SEO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.SEO;

public class RedirectConfiguration : BaseEntityConfiguration<Redirect>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Redirect> builder)
	{
		builder.ToTable("Redirects");

		builder.Property(x => x.FromPath).IsRequired().HasMaxLength(2000);
		builder.Property(x => x.ToPath).IsRequired().HasMaxLength(2000);
		builder.Property(x => x.Notes).HasMaxLength(1000);

		builder.HasIndex(x => x.FromPath)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Redirects_FromPath");

		builder.HasIndex(x => x.IsActive)
			.HasDatabaseName("IX_Redirects_Active");
	}
}