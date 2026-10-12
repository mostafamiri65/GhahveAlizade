using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public class UpdateCouponCommandHandler
	: IRequestHandler<UpdateCouponCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateCouponCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateCouponCommand request, CancellationToken ct)
	{
		var coupon = await _context.Coupons
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (coupon is null)
			return Error.NotFound("Coupon.NotFound", "کد تخفیف یافت نشد.");

		var code = request.Code.Trim().ToUpperInvariant();
		if (await _context.Coupons.AnyAsync(c => c.Code == code && c.Id != coupon.Id && !c.IsDeleted, ct))
			return Error.Conflict("Coupon.CodeExists", "این کد تخفیف قبلاً استفاده شده است.");

		coupon.Code = code;
		coupon.Description = request.Description?.Trim();
		coupon.DiscountType = request.DiscountType;
		coupon.DiscountValue = request.DiscountValue;
		coupon.MaxDiscountAmount = request.MaxDiscountAmount;
		coupon.MinOrderAmount = request.MinOrderAmount;
		coupon.MaxUsageCount = request.MaxUsageCount;
		coupon.MaxUsagePerCustomer = request.MaxUsagePerCustomer;
		coupon.StartsAtUtc = request.StartsAtUtc;
		coupon.ExpiresAtUtc = request.ExpiresAtUtc;
		coupon.IsActive = request.IsActive;
		coupon.IsFirstOrderOnly = request.IsFirstOrderOnly;
		coupon.IsPublic = request.IsPublic;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
