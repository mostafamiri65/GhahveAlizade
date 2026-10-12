using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler
	: IRequestHandler<DeleteReviewCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteReviewCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteReviewCommand request, CancellationToken ct)
	{
		var r = await _context.ProductReviews
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

		if (r is null) return Error.NotFound("Review.NotFound", "نظر یافت نشد.");

		_context.ProductReviews.Remove(r);

		// آپدیت آمار
		var others = await _context.ProductReviews
			.Where(x => x.ProductId == r.ProductId && x.Id != r.Id && x.IsApproved && !x.IsDeleted)
			.ToListAsync(ct);

		var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == r.ProductId, ct);
		if (product is not null)
		{
			product.ReviewCount = others.Count;
			product.AverageRating = others.Any()
				? (decimal)others.Average(x => x.Rating) : 0;
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}