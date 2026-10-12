using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Bundle;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public class CreateBundleCommandHandler
	: IRequestHandler<CreateBundleCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateBundleCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateBundleCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.ProductBundles.AnyAsync(b => b.Slug == slug && !b.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var bundle = new ProductBundle
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim(),
			ShortDescription = request.ShortDescription?.Trim(),
			ImageUrl = request.ImageUrl?.Trim(),
			BundlePrice = request.BundlePrice,
			CompareAtPrice = request.CompareAtPrice,
			TotalItemsPrice = request.TotalItemsPrice,
			IsActive = request.IsActive,
			IsFeatured = request.IsFeatured,
			StartsAtUtc = request.StartsAtUtc,
			ExpiresAtUtc = request.ExpiresAtUtc,
			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			CanonicalUrl = request.CanonicalUrl?.Trim(),
			OgImage = request.OgImage?.Trim()
		};

		foreach (var item in request.Items)
		{
			bundle.Items.Add(new ProductBundleItem
			{
				ProductId = item.ProductId,
				ProductVariantId = item.ProductVariantId,
				Quantity = item.Quantity,
				DisplayOrder = item.DisplayOrder
			});
		}

		_context.ProductBundles.Add(bundle);
		await _context.SaveChangesAsync(ct);
		return bundle.Id;
	}
}
