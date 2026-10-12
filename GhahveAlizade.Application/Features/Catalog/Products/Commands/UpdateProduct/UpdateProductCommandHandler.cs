using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
	: IRequestHandler<UpdateProductCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public UpdateProductCommandHandler(
		IAppDbContext context,
		IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(
		UpdateProductCommand request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.Include(p => p.Variants)
			.Include(p => p.Images)
			.Include(p => p.Attributes)
			.Include(p => p.FlavorNotes)
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		// Slug یکتا
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = await EnsureUniqueSlugAsync(baseSlug, product.Id, cancellationToken);

		var wasPublished = product.IsPublished;

		// آپدیت فیلدهای اصلی
		product.Name = request.Name.Trim();
		product.Slug = slug;
		product.ShortDescription = request.ShortDescription?.Trim();
		product.Description = request.Description?.Trim();
		product.Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
		product.Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();

		product.HasVariants = request.HasVariants;
		product.BasePrice = request.BasePrice;
		product.CompareAtPrice = request.CompareAtPrice;
		product.CostPrice = request.CostPrice;
		product.IsTaxable = request.IsTaxable;
		product.TaxRate = request.TaxRate;

		product.Weight = request.Weight;
		product.Length = request.Length;
		product.Width = request.Width;
		product.Height = request.Height;

		product.CategoryId = request.CategoryId;
		product.BrandId = request.BrandId;
		product.CoffeeOriginId = request.CoffeeOriginId;
		product.CoffeeProcessId = request.CoffeeProcessId;
		product.RoastProfileId = request.RoastProfileId;

		product.IsActive = request.IsActive;
		product.IsPublished = request.IsPublished;
		product.IsFeatured = request.IsFeatured;
		product.IsNew = request.IsNew;
		product.IsBestSeller = request.IsBestSeller;

		if (request.IsPublished && !wasPublished)
			product.PublishedAtUtc = _dateTime.UtcNow;

		product.MetaTitle = request.MetaTitle?.Trim();
		product.MetaDescription = request.MetaDescription?.Trim();
		product.MetaKeywords = request.MetaKeywords?.Trim();
		product.CanonicalUrl = request.CanonicalUrl?.Trim();
		product.OgImage = request.OgImage?.Trim();

		// ============ Variants ============
		// استراتژی: همه رو پاک کن و از نو بساز (ساده‌تر از diff کردن)
		foreach (var v in product.Variants.ToList())
			_context.ProductVariants.Remove(v);

		foreach (var v in request.Variants)
		{
			product.Variants.Add(new ProductVariant
			{
				ProductId = product.Id,
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

		// ============ Images ============
		foreach (var img in product.Images.ToList())
			_context.ProductImages.Remove(img);

		foreach (var img in request.Images)
		{
			product.Images.Add(new ProductImage
			{
				ProductId = product.Id,
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

		// ============ Attributes ============
		foreach (var attr in product.Attributes.ToList())
			_context.ProductAttributes.Remove(attr);

		foreach (var attr in request.Attributes)
		{
			product.Attributes.Add(new ProductAttribute
			{
				ProductId = product.Id,
				AttributeId = attr.AttributeId,
				AttributeValueId = attr.AttributeValueId,
				CustomValue = attr.CustomValue,
				DisplayOrder = attr.DisplayOrder
			});
		}

		// ============ FlavorNotes ============
		product.FlavorNotes.Clear();

		if (request.FlavorNoteIds.Any())
		{
			var flavorNotes = await _context.FlavorNotes
				.Where(f => request.FlavorNoteIds.Contains(f.Id) && !f.IsDeleted)
				.ToListAsync(cancellationToken);

			foreach (var note in flavorNotes)
				product.FlavorNotes.Add(note);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success;
	}

	private async Task<string> EnsureUniqueSlugAsync(
		string baseSlug, long excludeId, CancellationToken ct)
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