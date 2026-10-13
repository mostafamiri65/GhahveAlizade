using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetTicketDetail;

public class GetTicketDetailQueryHandler
	: IRequestHandler<GetTicketDetailQuery, ErrorOr<TicketDetailWithMetaDto>>
{
	private readonly IAppDbContext _context;

	public GetTicketDetailQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<TicketDetailWithMetaDto>> Handle(GetTicketDetailQuery request, CancellationToken ct)
	{
		var ticket = await _context.SupportTickets
			.AsNoTracking()
			.Include(t => t.Customer)
			.Include(t => t.AssignedTo)
			.Include(t => t.Messages.OrderBy(m => m.CreatedAtUtc)) // ادمین InternalNote را هم می‌بیند
			.Where(t => t.Id == request.TicketId && !t.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (ticket is null)
			return Error.NotFound("Ticket.NotFound", "تیکت یافت نشد.");

		// علامت‌گذاری پیام‌های مشتری به‌عنوان خوانده‌شده
		var unread = await _context.TicketMessages
			.Where(m => m.TicketId == ticket.Id && !m.IsFromAdmin && m.ReadAtUtc == null && !m.IsDeleted)
			.ToListAsync(ct);
		foreach (var m in unread)
			m.ReadAtUtc = DateTime.UtcNow;
		if (unread.Count > 0)
			await _context.SaveChangesAsync(ct);

		return new TicketDetailWithMetaDto
		{
			Id = ticket.Id,
			TicketNumber = ticket.TicketNumber,
			Subject = ticket.Subject,
			Category = ticket.Category,
			Status = ticket.Status,
			RelatedOrderId = ticket.RelatedOrderId,
			CreatedAtUtc = ticket.CreatedAtUtc,
			CustomerName = ticket.Customer != null
				? (ticket.Customer.FirstName + " " + ticket.Customer.LastName).Trim()
				: ticket.GuestName,
			CustomerPhone = ticket.Customer != null ? ticket.Customer.PhoneNumber : ticket.GuestPhoneNumber,
			AssignedToUserId = ticket.AssignedToUserId,
			AssignedToName = ticket.AssignedTo != null
				? (ticket.AssignedTo.FirstName + " " + ticket.AssignedTo.LastName).Trim()
				: null,
			Messages = ticket.Messages.Select(m => new TicketMessageDto
			{
				Id = m.Id,
				SenderName = m.SenderName,
				IsFromAdmin = m.IsFromAdmin,
				Body = m.Body,
				CreatedAtUtc = m.CreatedAtUtc
			}).ToList()
		};
	}
}
