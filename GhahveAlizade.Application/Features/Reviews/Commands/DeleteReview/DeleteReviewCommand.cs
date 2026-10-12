using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(long Id) : IRequest<ErrorOr<Success>>;