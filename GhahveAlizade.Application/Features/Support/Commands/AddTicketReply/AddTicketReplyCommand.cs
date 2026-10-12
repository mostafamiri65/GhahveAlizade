using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.AddTicketReply;

public record AddTicketReplyCommand(
	long UserId,          // فرستنده (کاربر یا ادمین)
	long TicketId,
	string Body,
	bool IsFromAdmin = false) : IRequest<ErrorOr<Success>>;
