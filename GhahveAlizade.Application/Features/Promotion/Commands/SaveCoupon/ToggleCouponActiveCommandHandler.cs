using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public class ToggleCouponActiveCommandHandler
	: IRequestHandler<ToggleCouponActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleCouponActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleCouponActiveCommand request, CancellationToken ct)
	{
		var coupon = await _context.Coupons
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (coupon is null)
			return Error.NotFound("Coupon.NotFound", "کد تخفیف یافت نشد.");

		coupon.IsActive = !coupon.IsActive;
		await _context.SaveChangesAsync(ct);
		return coupon.IsActive;
	}
}