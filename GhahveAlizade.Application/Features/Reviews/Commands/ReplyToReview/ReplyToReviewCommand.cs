using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Reviews.Commands.ReplyToReview;

public record ReplyToReviewCommand(
	long ReviewId,
	string Content,
	bool IsOfficial) : IRequest<ErrorOr<long>>;