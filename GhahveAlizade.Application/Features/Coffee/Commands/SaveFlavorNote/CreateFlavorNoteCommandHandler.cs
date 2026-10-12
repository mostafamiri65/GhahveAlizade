using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public class CreateFlavorNoteCommandHandler
	: IRequestHandler<CreateFlavorNoteCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateFlavorNoteCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateFlavorNoteCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.FlavorNotes.AnyAsync(f => f.Slug == slug && !f.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var note = new FlavorNote
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Category = request.Category?.Trim(),
			IconUrl = request.IconUrl?.Trim(),
			ColorCode = request.ColorCode?.Trim(),
			IsActive = request.IsActive,
			DisplayOrder = request.DisplayOrder
		};

		_context.FlavorNotes.Add(note);
		await _context.SaveChangesAsync(ct);
		return note.Id;
	}
}
