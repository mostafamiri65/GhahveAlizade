using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.DeleteCustomerNote;

public class DeleteCustomerNoteCommandHandler
	: IRequestHandler<DeleteCustomerNoteCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteCustomerNoteCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteCustomerNoteCommand request, CancellationToken ct)
	{
		var note = await _context.CustomerNotes
			.FirstOrDefaultAsync(n => n.Id == request.Id && !n.IsDeleted, ct);

		if (note is null)
			return Error.NotFound("CustomerNote.NotFound", "یادداشت یافت نشد.");

		_context.CustomerNotes.Remove(note);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}