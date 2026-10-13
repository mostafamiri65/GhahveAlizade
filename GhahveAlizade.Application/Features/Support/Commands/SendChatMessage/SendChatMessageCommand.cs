using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.SendChatMessage;

public record SendChatMessageCommand(
	long SenderUserId,
	long SessionId,
	string Body,
	bool IsFromAdmin = false) : IRequest<ErrorOr<ChatMessageDto>>;
