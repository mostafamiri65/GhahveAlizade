using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
	: IRequestHandler<CreateProductCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public CreateProductCommandHandler(
		IAppDbContext context,
		IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<long>> Handle(
		CreateProductCommand request,
		CancellationToken cancellationToken)
	{
		// Slug یکتا
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

		var product = new Product
		{
			Name = request.Name.Trim(),
			Slug = slug,
			ShortDescription = request.ShortDescription?.Trim(),
			Description = request.Description?.Trim(),
			Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim(),
			Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim(),

			HasVariants = request.HasVariants,
			BasePrice = request.BasePrice,
			CompareAtPrice = request.CompareAtPrice,
			CostPrice = request.CostPrice,
			IsTaxable = request.IsTaxable,
			TaxRate = request.TaxRate,

			Weight = request.Weight,
			Length = request.Length,
			Width = request.Width,
			Height = request.Height,

			CategoryId = request.CategoryId,
			BrandId = request.BrandId,
			CoffeeOriginId = request.CoffeeOriginId,
			CoffeeProcessId = request.CoffeeProcessId,
			RoastProfileId = request.RoastProfileId,

			IsActive = request.IsActive,
			IsPublished = request.IsPublished,
			IsFeatured = request.IsFeatured,
			IsNew = request.IsNew,
			IsBestSeller = request.IsBestSeller,
			PublishedAtUtc = request.IsPublished ? _dateTime.UtcNow : null,

			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			MetaKeywords = request.MetaKeywords?.Trim(),
			CanonicalUrl = request.CanonicalUrl?.Trim(),
			OgImage = request.OgImage?.Trim()
		};

		// Variants
		foreach (var v in request.Variants)
		{
			product.Variants.Add(new ProductVariant
			{
				Name = v.Name.Trim(),
				Sku = string.IsNullOrWhiteSpace(v.Sku) ? null : v.Sku.Trim(),
				Barcode = string.IsNullOrWhiteSpace(v.Barcode) ? null : v.Barcode.Trim(),
				AttributesJson = v.AttributesJson,
				Price = v.Price,
				CompareAtPrice = v.CompareAtPrice,
				CostPrice = v.CostPrice,
				Weight = v.Weight,
				ImageUrl = v.ImageUrl,
				DisplayOrder = v.DisplayOrder,
				IsActive = v.IsActive,
				IsDefault = v.IsDefault
			});
		}

		// Images
		foreach (var img in request.Images)
		{
			product.Images.Add(new ProductImage
			{
				ImageUrl = img.ImageUrl,
				ThumbnailUrl = img.ThumbnailUrl,
				AltText = img.AltText,
				Title = img.Title,
				DisplayOrder = img.DisplayOrder,
				IsPrimary = img.IsPrimary,
				FileSizeBytes = img.FileSizeBytes,
				Width = img.Width,
				Height = img.Height
			});
		}

		// Attributes
		foreach (var attr in request.Attributes)
		{
			product.Attributes.Add(new ProductAttribute
			{
				AttributeId = attr.AttributeId,
				AttributeValueId = attr.AttributeValueId,
				CustomValue = attr.CustomValue,
				DisplayOrder = attr.DisplayOrder
			});
		}

		// FlavorNotes
		if (request.FlavorNoteIds.Any())
		{
			var flavorNotes = await _context.FlavorNotes
				.Where(f => request.FlavorNoteIds.Contains(f.Id) && !f.IsDeleted)
				.ToListAsync(cancellationToken);

			foreach (var note in flavorNotes)
				product.FlavorNotes.Add(note);
		}

		_context.Products.Add(product);
		await _context.SaveChangesAsync(cancellationToken);

		return product.Id;
	}

	private async Task<string> EnsureUniqueSlugAsync(
		string baseSlug, long? excludeId, CancellationToken ct)
	{
		var slug = baseSlug;
		var counter = 1;

		while (await _context.Products.AnyAsync(
			p => p.Slug == slug && p.Id != excludeId && !p.IsDeleted, ct))
		{
			slug = $"{baseSlug}-{counter}";
			counter++;
		}

		return slug;
	}
}