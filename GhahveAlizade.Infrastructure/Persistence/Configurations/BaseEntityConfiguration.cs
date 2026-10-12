using GhahveAlizade.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
	where TEntity : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedOnAdd();

		builder.Property(x => x.CreatedAtUtc)
			.IsRequired();


		builder.Property(x => x.RowVersion)
			.IsRowVersion()
			.IsConcurrencyToken();

		builder.HasIndex(x => x.CreatedAtUtc)
			.HasDatabaseName($"IX_{typeof(TEntity).Name}_CreatedAtUtc");

		ConfigureEntity(builder);
	}

	protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}