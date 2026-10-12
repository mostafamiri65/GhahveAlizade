using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Bundle;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public class UpdateBundleCommandHandler
	: IRequestHandler<UpdateBundleCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateBundleCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateBundleCommand request, CancellationToken ct)
	{
		var bundle = await _context.ProductBundles
			.Include(b => b.Items)
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (bundle is null) return Error.NotFound("Bundle.NotFound", "بسته یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.ProductBundles.AnyAsync(
			b => b.Slug == slug && b.Id != bundle.Id && !b.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		bundle.Name = request.Name.Trim();
		bundle.Slug = slug;
		bundle.Description = request.Description?.Trim();
		bundle.ShortDescription = request.ShortDescription?.Trim();
		bundle.ImageUrl = request.ImageUrl?.Trim();
		bundle.BundlePrice = request.BundlePrice;
		bundle.CompareAtPrice = request.CompareAtPrice;
		bundle.TotalItemsPrice = request.TotalItemsPrice;
		bundle.IsActive = request.IsActive;
		bundle.IsFeatured = request.IsFeatured;
		bundle.StartsAtUtc = request.StartsAtUtc;
		bundle.ExpiresAtUtc = request.ExpiresAtUtc;
		bundle.MetaTitle = request.MetaTitle?.Trim();
		bundle.MetaDescription = request.MetaDescription?.Trim();
		bundle.CanonicalUrl = request.CanonicalUrl?.Trim();
		bundle.OgImage = request.OgImage?.Trim();

		foreach (var i in bundle.Items.ToList())
			_context.ProductBundleItems.Remove(i);

		foreach (var item in request.Items)
		{
			bundle.Items.Add(new ProductBundleItem
			{
				ProductBundleId = bundle.Id,
				ProductId = item.ProductId,
				ProductVariantId = item.ProductVariantId,
				Quantity = item.Quantity,
				DisplayOrder = item.DisplayOrder
			});
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
