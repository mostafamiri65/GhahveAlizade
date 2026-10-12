using ErrorOr;
using GhahveAlizade.Application.Features.Reviews.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reviews.Queries.GetReviewById;

public record GetReviewByIdQuery(long Id) : IRequest<ErrorOr<ReviewDetailDto>>;