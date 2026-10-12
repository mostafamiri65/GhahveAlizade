using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public class ToggleFlavorNoteActiveCommandHandler
	: IRequestHandler<ToggleFlavorNoteActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleFlavorNoteActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleFlavorNoteActiveCommand request, CancellationToken ct)
	{
		var note = await _context.FlavorNotes
			.FirstOrDefaultAsync(f => f.Id == request.Id && !f.IsDeleted, ct);

		if (note is null)
			return Error.NotFound("FlavorNote.NotFound", "نُت طعمی یافت نشد.");

		note.IsActive = !note.IsActive;
		await _context.SaveChangesAsync(ct);
		return note.IsActive;
	}
}