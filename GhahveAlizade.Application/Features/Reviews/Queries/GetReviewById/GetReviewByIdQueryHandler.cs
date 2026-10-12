using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reviews.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reviews.Queries.GetReviewById;

public class GetReviewByIdQueryHandler
	: IRequestHandler<GetReviewByIdQuery, ErrorOr<ReviewDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetReviewByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ReviewDetailDto>> Handle(GetReviewByIdQuery request, CancellationToken ct)
	{
		var r = await _context.ProductReviews
			.AsNoTracking()
			.Include(x => x.Product)
			.Include(x => x.Customer)
			.Include(x => x.Images)
			.Include(x => x.Replies)
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (r is null)
			return Error.NotFound("Review.NotFound", "نظر یافت نشد.");

		return r.Adapt<ReviewDetailDto>();
	}
}