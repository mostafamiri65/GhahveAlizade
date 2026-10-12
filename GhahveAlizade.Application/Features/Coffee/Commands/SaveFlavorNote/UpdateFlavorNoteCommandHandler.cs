using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public class UpdateFlavorNoteCommandHandler
	: IRequestHandler<UpdateFlavorNoteCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateFlavorNoteCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateFlavorNoteCommand request, CancellationToken ct)
	{
		var note = await _context.FlavorNotes
			.FirstOrDefaultAsync(f => f.Id == request.Id && !f.IsDeleted, ct);

		if (note is null)
			return Error.NotFound("FlavorNote.NotFound", "نُت طعمی یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.FlavorNotes.AnyAsync(f => f.Slug == slug && f.Id != note.Id && !f.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		note.Name = request.Name.Trim();
		note.Slug = slug;
		note.Category = request.Category?.Trim();
		note.IconUrl = request.IconUrl?.Trim();
		note.ColorCode = request.ColorCode?.Trim();
		note.IsActive = request.IsActive;
		note.DisplayOrder = request.DisplayOrder;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
