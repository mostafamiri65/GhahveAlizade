using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Promotion.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Queries.GetCouponById;

public class GetCouponByIdQueryHandler
	: IRequestHandler<GetCouponByIdQuery, ErrorOr<CouponDto>>
{
	private readonly IAppDbContext _context;

	public GetCouponByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken ct)
	{
		var c = await _context.Coupons.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<CouponDto>()
			.FirstOrDefaultAsync(ct);

		if (c is null) return Error.NotFound("Coupon.NotFound", "کد تخفیف یافت نشد.");
		return c;
	}
}