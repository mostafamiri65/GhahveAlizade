using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GhahveAlizade.Infrastructure.Persistence.Configurations.Catalog;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
	protected override void ConfigureEntity(EntityTypeBuilder<Product> builder)
	{
		builder.ToTable("Products");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.ShortDescription)
			.HasMaxLength(1000);

		builder.Property(x => x.Description)
			.HasColumnType("nvarchar(max)");

		builder.Property(x => x.Sku)
			.HasMaxLength(100);

		builder.Property(x => x.Barcode)
			.HasMaxLength(100);

		builder.Property(x => x.BasePrice)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.CompareAtPrice)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.CostPrice)
			.HasColumnType("decimal(18,2)");

		builder.Property(x => x.TaxRate)
			.HasColumnType("decimal(5,2)");

		builder.Property(x => x.Weight)
			.HasColumnType("decimal(10,2)");

		builder.Property(x => x.Length)
			.HasColumnType("decimal(10,2)");

		builder.Property(x => x.Width)
			.HasColumnType("decimal(10,2)");

		builder.Property(x => x.Height)
			.HasColumnType("decimal(10,2)");

		builder.Property(x => x.AverageRating)
			.HasColumnType("decimal(3,2)");

		// SEO
		builder.Property(x => x.MetaTitle)
			.HasMaxLength(200);

		builder.Property(x => x.MetaDescription)
			.HasMaxLength(500);

		builder.Property(x => x.MetaKeywords)
			.HasMaxLength(500);

		builder.Property(x => x.CanonicalUrl)
			.HasMaxLength(1000);

		builder.Property(x => x.OgImage)
			.HasMaxLength(1000);

		builder.Property(x => x.JsonLdSchema)
			.HasColumnType("nvarchar(max)");

		// Indexes برای SEO و Performance
		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasFilter("[IsDeleted] = 0")
			.HasDatabaseName("UX_Products_Slug");

		builder.HasIndex(x => x.Sku)
			.IsUnique()
			.HasFilter("[Sku] IS NOT NULL AND [IsDeleted] = 0")
			.HasDatabaseName("UX_Products_Sku");

		builder.HasIndex(x => new { x.IsPublished, x.IsActive, x.IsDeleted })
			.HasDatabaseName("IX_Products_Status");

		builder.HasIndex(x => new { x.CategoryId, x.IsPublished })
			.HasDatabaseName("IX_Products_Category_Published");

		builder.HasIndex(x => new { x.BrandId, x.IsPublished })
			.HasDatabaseName("IX_Products_Brand_Published");

		builder.HasIndex(x => new { x.IsFeatured, x.IsPublished, x.IsActive })
			.HasDatabaseName("IX_Products_Featured");

		builder.HasIndex(x => x.PublishedAtUtc)
			.HasDatabaseName("IX_Products_PublishedAt");

		// Relations
		builder.HasOne(x => x.Category)
			.WithMany(c => c.Products)
			.HasForeignKey(x => x.CategoryId)
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(x => x.Brand)
			.WithMany(b => b.Products)
			.HasForeignKey(x => x.BrandId)
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(x => x.CoffeeOrigin)
			.WithMany()
			.HasForeignKey(x => x.CoffeeOriginId)
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(x => x.CoffeeProcess)
			.WithMany()
			.HasForeignKey(x => x.CoffeeProcessId)
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasOne(x => x.RoastProfile)
			.WithMany()
			.HasForeignKey(x => x.RoastProfileId)
			.OnDelete(DeleteBehavior.SetNull);

		builder.HasMany(x => x.FlavorNotes)
			.WithMany(f => f.Products)
			.UsingEntity(j => j.ToTable("ProductFlavorNotes"));
	}
}