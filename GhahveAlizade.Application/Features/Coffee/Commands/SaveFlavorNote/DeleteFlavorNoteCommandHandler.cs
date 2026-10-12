using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public class DeleteFlavorNoteCommandHandler
	: IRequestHandler<DeleteFlavorNoteCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteFlavorNoteCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteFlavorNoteCommand request, CancellationToken ct)
	{
		var note = await _context.FlavorNotes
			.FirstOrDefaultAsync(f => f.Id == request.Id && !f.IsDeleted, ct);

		if (note is null)
			return Error.NotFound("FlavorNote.NotFound", "نُت طعمی یافت نشد.");

		_context.FlavorNotes.Remove(note);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
