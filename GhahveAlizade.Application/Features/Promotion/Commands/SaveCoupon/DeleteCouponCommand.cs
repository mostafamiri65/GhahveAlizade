using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public record DeleteCouponCommand(long Id) : IRequest<ErrorOr<Success>>;
