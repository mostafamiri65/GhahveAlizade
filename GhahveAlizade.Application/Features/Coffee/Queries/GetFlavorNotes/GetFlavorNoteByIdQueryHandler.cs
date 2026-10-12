using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetFlavorNotes;

public class GetFlavorNoteByIdQueryHandler
	: IRequestHandler<GetFlavorNoteByIdQuery, ErrorOr<FlavorNoteDto>>
{
	private readonly IAppDbContext _context;
	public GetFlavorNoteByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<FlavorNoteDto>> Handle(GetFlavorNoteByIdQuery request, CancellationToken ct)
	{
		var f = await _context.FlavorNotes.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<FlavorNoteDto>()
			.FirstOrDefaultAsync(ct);

		if (f is null)
			return Error.NotFound("FlavorNote.NotFound", "نُت طعمی یافت نشد.");

		return f;
	}
}