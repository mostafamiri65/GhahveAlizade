using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.AiAdvisor.Commands.SendAiMessage;

public record SendAiMessageCommand(long UserId, string Message)
	: IRequest<ErrorOr<SendAiMessageResponse>>;

public record SendAiMessageResponse(string Reply, long MessageId);
