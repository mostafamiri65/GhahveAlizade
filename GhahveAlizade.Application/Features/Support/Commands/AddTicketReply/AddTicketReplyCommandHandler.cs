using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.AddTicketReply;

public class AddTicketReplyCommandHandler : IRequestHandler<AddTicketReplyCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public AddTicketReplyCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(AddTicketReplyCommand request, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(request.Body) || request.Body.Trim().Length < 2)
			return Error.Validation("Ticket.ReplyRequired", "متن پاسخ را وارد کنید.");

		var userIdStr = request.UserId.ToString();

		var ticket = await _context.SupportTickets
			.Include(t => t.Messages)
			.Where(t => t.Id == request.TicketId && !t.IsDeleted
				&& (request.IsFromAdmin || t.UserId == userIdStr)) // کاربر فقط تیکت خودش
			.FirstOrDefaultAsync(ct);

		if (ticket is null)
			return Error.NotFound("Ticket.NotFound", "تیکت یافت نشد.");

		if (ticket.Status == TicketStatus.Closed)
			return Error.Conflict("Ticket.Closed", "این تیکت بسته شده است.");

		ticket.Messages.Add(new TicketMessage
		{
			Body = request.Body.Trim(),
			SenderUserId = request.UserId,
			SenderName = request.IsFromAdmin ? "پشتیبانی" : "مشتری",
			IsFromAdmin = request.IsFromAdmin,
			IsInternalNote = request.IsInternalNote
		});

		// اولین پاسخ ادمین
		if (request.IsFromAdmin && ticket.FirstResponseAtUtc is null)
			ticket.FirstResponseAtUtc = DateTime.UtcNow;

		// اگر ادمین پاسخ داد → منتظر مشتری (مگر یادداشت داخلی باشد)
		if (request.IsFromAdmin && !request.IsInternalNote)
			ticket.Status = TicketStatus.WaitingCustomer;

		// اگر مشتری پاسخ داد و تیکت منتظر مشتری بود → در حال بررسی
		if (!request.IsFromAdmin && ticket.Status == TicketStatus.WaitingCustomer)
			ticket.Status = TicketStatus.InProgress;

		ticket.UpdatedAtUtc = DateTime.UtcNow;
		await _context.SaveChangesAsync(ct);

		return Result.Success;
	}
}
