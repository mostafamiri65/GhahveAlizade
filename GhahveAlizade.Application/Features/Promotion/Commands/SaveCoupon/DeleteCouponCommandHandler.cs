using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public class DeleteCouponCommandHandler
	: IRequestHandler<DeleteCouponCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteCouponCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteCouponCommand request, CancellationToken ct)
	{
		var coupon = await _context.Coupons
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (coupon is null)
			return Error.NotFound("Coupon.NotFound", "کد تخفیف یافت نشد.");

		if (coupon.UsedCount > 0)
			return Error.Conflict("Coupon.InUse",
				"این کد تخفیف استفاده شده و قابل حذف نیست. می‌توانید آن را غیرفعال کنید.");

		_context.Coupons.Remove(coupon);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
