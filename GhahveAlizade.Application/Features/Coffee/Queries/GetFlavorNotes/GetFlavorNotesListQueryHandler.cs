using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetFlavorNotes;

public class GetFlavorNotesListQueryHandler
	: IRequestHandler<GetFlavorNotesListQuery, ErrorOr<PaginatedList<FlavorNoteListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetFlavorNotesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<FlavorNoteListItemDto>>> Handle(
		GetFlavorNotesListQuery request, CancellationToken ct)
	{
		var query = _context.FlavorNotes.AsNoTracking().Where(f => !f.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(f => f.Name.Contains(s) || f.Slug.Contains(s));
		}

		if (!string.IsNullOrWhiteSpace(request.Category))
			query = query.Where(f => f.Category == request.Category);

		if (request.IsActive.HasValue)
			query = query.Where(f => f.IsActive == request.IsActive.Value);

		query = request.SortBy?.ToLower() switch
		{
			"name" => request.SortDir == "desc" ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name),
			"category" => request.SortDir == "desc" ? query.OrderByDescending(f => f.Category) : query.OrderBy(f => f.Category),
			_ => query.OrderBy(f => f.Category).ThenBy(f => f.DisplayOrder)
		};

		return await PaginatedList<FlavorNoteListItemDto>.CreateAsync(
			query.ProjectToType<FlavorNoteListItemDto>(), request.PageNumber, request.PageSize, ct);
	}
}
