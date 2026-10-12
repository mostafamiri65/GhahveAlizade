using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;
public record ToggleCouponActiveCommand(long Id) : IRequest<ErrorOr<bool>>;