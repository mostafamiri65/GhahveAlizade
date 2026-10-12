using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Review;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reviews.Commands.ReplyToReview;

public class ReplyToReviewCommandHandler
	: IRequestHandler<ReplyToReviewCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public ReplyToReviewCommandHandler(
		IAppDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<long>> Handle(ReplyToReviewCommand request, CancellationToken ct)
	{
		var review = await _context.ProductReviews
			.FirstOrDefaultAsync(x => x.Id == request.ReviewId && !x.IsDeleted, ct);

		if (review is null) return Error.NotFound("Review.NotFound", "نظر یافت نشد.");

		var reply = new ReviewReply
		{
			ProductReviewId = review.Id,
			Content = request.Content.Trim(),
			RepliedByUserId = _currentUser.UserId?.ToString(),
			RepliedByName = _currentUser.UserName,
			IsOfficial = request.IsOfficial,
			IsApproved = true
		};

		_context.ReviewReplies.Add(reply);
		await _context.SaveChangesAsync(ct);
		return reply.Id;
	}
}