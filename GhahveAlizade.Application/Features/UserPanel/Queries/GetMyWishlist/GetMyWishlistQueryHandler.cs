using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyWishlist;

public class GetMyWishlistQueryHandler
	: IRequestHandler<GetMyWishlistQuery, ErrorOr<List<MyWishlistItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetMyWishlistQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<MyWishlistItemDto>>> Handle(GetMyWishlistQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var items = await _context.WishlistItems
			.AsNoTracking()
			.Include(i => i.Wishlist)
			.Include(i => i.Product)
				.ThenInclude(p => p.Images)
			.Where(i => !i.IsDeleted
				&& i.Wishlist != null
				&& !i.Wishlist.IsDeleted
				&& i.Wishlist.UserId == userIdStr)
			.OrderByDescending(i => i.CreatedAtUtc)
			.ProjectToType<MyWishlistItemDto>()
			.ToListAsync(ct);

		return items;
	}
}
