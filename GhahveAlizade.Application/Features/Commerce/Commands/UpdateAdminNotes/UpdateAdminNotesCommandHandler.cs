using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Commerce.Commands.UpdateAdminNotes;

public class UpdateAdminNotesCommandHandler
	: IRequestHandler<UpdateAdminNotesCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateAdminNotesCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateAdminNotesCommand request, CancellationToken ct)
	{
		var order = await _context.Orders
			.FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, ct);

		if (order is null) return Error.NotFound("Order.NotFound", "سفارش یافت نشد.");

		order.AdminNotes = request.AdminNotes?.Trim();
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}