using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Promotion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public class CreateCouponCommandHandler
	: IRequestHandler<CreateCouponCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateCouponCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateCouponCommand request, CancellationToken ct)
	{
		var code = request.Code.Trim().ToUpperInvariant();

		if (await _context.Coupons.AnyAsync(c => c.Code == code && !c.IsDeleted, ct))
			return Error.Conflict("Coupon.CodeExists", "این کد تخفیف قبلاً استفاده شده است.");

		var coupon = new Coupon
		{
			Code = code,
			Description = request.Description?.Trim(),
			DiscountType = request.DiscountType,
			DiscountValue = request.DiscountValue,
			MaxDiscountAmount = request.MaxDiscountAmount,
			MinOrderAmount = request.MinOrderAmount,
			MaxUsageCount = request.MaxUsageCount,
			MaxUsagePerCustomer = request.MaxUsagePerCustomer,
			StartsAtUtc = request.StartsAtUtc,
			ExpiresAtUtc = request.ExpiresAtUtc,
			IsActive = request.IsActive,
			IsFirstOrderOnly = request.IsFirstOrderOnly,
			IsPublic = request.IsPublic
		};

		_context.Coupons.Add(coupon);
		await _context.SaveChangesAsync(ct);
		return coupon.Id;
	}
}
