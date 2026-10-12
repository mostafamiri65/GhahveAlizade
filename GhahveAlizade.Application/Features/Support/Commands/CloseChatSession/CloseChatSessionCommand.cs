using ErrorOr;
using GhahveAlizade.Domain.Support;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.CloseChatSession;

public record CloseChatSessionCommand(
	long UserId,
	long SessionId,
	bool IsFromAdmin = false) : IRequest<ErrorOr<Success>>;
