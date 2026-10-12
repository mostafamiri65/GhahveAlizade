using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reviews.Commands.ApproveReview;

public class ApproveReviewCommandHandler
	: IRequestHandler<ApproveReviewCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ApproveReviewCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ApproveReviewCommand request, CancellationToken ct)
	{
		var r = await _context.ProductReviews
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

		if (r is null) return Error.NotFound("Review.NotFound", "نظر یافت نشد.");

		r.IsApproved = !r.IsApproved;
		r.ApprovedAtUtc = r.IsApproved ? DateTime.UtcNow : null;

		// آپدیت آمار محصول
		await UpdateProductStatsAsync(r.ProductId, ct);

		await _context.SaveChangesAsync(ct);
		return r.IsApproved;
	}

	private async Task UpdateProductStatsAsync(long productId, CancellationToken ct)
	{
		var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId, ct);
		if (product is null) return;

		var approved = await _context.ProductReviews
			.Where(r => r.ProductId == productId && r.IsApproved && !r.IsDeleted)
			.ToListAsync(ct);

		product.ReviewCount = approved.Count;
		product.AverageRating = approved.Any()
			? (decimal)approved.Average(r => r.Rating)
			: 0;
	}
}