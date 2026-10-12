using ErrorOr;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Commerce.Commands.ChangeOrderStatus;

public record ChangeOrderStatusCommand(
	long OrderId,
	OrderStatus NewStatus,
	string? Note = null) : IRequest<ErrorOr<Success>>;