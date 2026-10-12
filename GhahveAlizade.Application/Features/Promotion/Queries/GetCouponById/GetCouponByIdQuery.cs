using ErrorOr;
using GhahveAlizade.Application.Features.Promotion.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Promotion.Queries.GetCouponById;

public record GetCouponByIdQuery(long Id) : IRequest<ErrorOr<CouponDto>>;