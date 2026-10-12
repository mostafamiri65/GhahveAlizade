using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataById;

public class GetSeoMetadataByIdQueryHandler
	: IRequestHandler<GetSeoMetadataByIdQuery, ErrorOr<SeoMetadataDto>>
{
	private readonly IAppDbContext _context;
	public GetSeoMetadataByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<SeoMetadataDto>> Handle(GetSeoMetadataByIdQuery request, CancellationToken ct)
	{
		var s = await _context.SeoMetadata.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<SeoMetadataDto>()
			.FirstOrDefaultAsync(ct);

		if (s is null) return Error.NotFound("Seo.NotFound", "متادیتای سئو یافت نشد.");
		return s;
	}
}