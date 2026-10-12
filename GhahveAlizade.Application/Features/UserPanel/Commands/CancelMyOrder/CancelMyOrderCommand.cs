using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.CancelMyOrder;

public record CancelMyOrderCommand(long UserId, long OrderId, string? Reason)
	: IRequest<ErrorOr<Success>>;
