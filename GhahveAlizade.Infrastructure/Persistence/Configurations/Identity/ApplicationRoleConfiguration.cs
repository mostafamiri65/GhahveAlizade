using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
	public void Configure(EntityTypeBuilder<ApplicationRole> builder)
	{
		builder.ToTable("Roles");

		builder.Property(x => x.Description).HasMaxLength(1000);

		builder.HasIndex(x => x.IsSystemRole)
			.HasDatabaseName("IX_Roles_IsSystemRole");
	}
}