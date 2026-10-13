using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyTicketById;

public class GetMyTicketByIdQueryHandler
	: IRequestHandler<GetMyTicketByIdQuery, ErrorOr<TicketDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetMyTicketByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<TicketDetailDto>> Handle(GetMyTicketByIdQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var ticket = await _context.SupportTickets
			.AsNoTracking()
			.Include(t => t.Messages.Where(m => !m.IsInternalNote)
				.OrderBy(m => m.CreatedAtUtc))
			.Where(t => t.Id == request.TicketId && t.UserId == userIdStr) // ضد IDOR
			.FirstOrDefaultAsync(ct);

		if (ticket is null)
			return Error.NotFound("Ticket.NotFound", "تیکت یافت نشد.");

		// علامت‌گذاری پاسخ‌های ادمین به‌عنوان خوانده‌شده
		var unread = await _context.TicketMessages
			.Where(m => m.TicketId == ticket.Id && m.IsFromAdmin && m.ReadAtUtc == null && !m.IsDeleted)
			.ToListAsync(ct);
		foreach (var m in unread)
			m.ReadAtUtc = DateTime.UtcNow;
		if (unread.Count > 0)
			await _context.SaveChangesAsync(ct);

		return new TicketDetailDto
		{
			Id = ticket.Id,
			TicketNumber = ticket.TicketNumber,
			Subject = ticket.Subject,
			Category = ticket.Category,
			Status = ticket.Status,
			RelatedOrderId = ticket.RelatedOrderId,
			CreatedAtUtc = ticket.CreatedAtUtc,
			Messages = ticket.Messages.Adapt<List<TicketMessageDto>>()
		};
	}
}
