using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Reviews.Commands.ApproveReview;

public record ApproveReviewCommand(long Id) : IRequest<ErrorOr<bool>>;