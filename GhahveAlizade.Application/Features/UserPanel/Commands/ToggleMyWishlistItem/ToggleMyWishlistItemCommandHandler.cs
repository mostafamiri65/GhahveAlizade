using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Wishlist;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.ToggleMyWishlistItem;

public class ToggleMyWishlistItemCommandHandler
	: IRequestHandler<ToggleMyWishlistItemCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleMyWishlistItemCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleMyWishlistItemCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var productExists = await _context.Products
			.AnyAsync(p => p.Id == request.ProductId && !p.IsDeleted, ct);
		if (!productExists)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		var wishlist = await _context.Wishlists
			.Include(w => w.Items)
			.Where(w => !w.IsDeleted && w.UserId == userIdStr)
			.FirstOrDefaultAsync(ct);

		if (wishlist is null)
		{
			wishlist = new Wishlist { UserId = userIdStr };
			_context.Wishlists.Add(wishlist);
			await _context.SaveChangesAsync(ct);
		}

		var existing = wishlist.Items.FirstOrDefault(i => !i.IsDeleted && i.ProductId == request.ProductId);
		if (existing is not null)
		{
			existing.IsDeleted = true;
			existing.DeletedAtUtc = DateTime.UtcNow;
			await _context.SaveChangesAsync(ct);
			return false;
		}

		_context.WishlistItems.Add(new WishlistItem
		{
			WishlistId = wishlist.Id,
			ProductId = request.ProductId
		});
		await _context.SaveChangesAsync(ct);
		return true;
	}
}
