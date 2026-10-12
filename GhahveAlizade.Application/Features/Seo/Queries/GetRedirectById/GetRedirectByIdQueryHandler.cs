using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetRedirectById;

public class GetRedirectByIdQueryHandler
	: IRequestHandler<GetRedirectByIdQuery, ErrorOr<RedirectDto>>
{
	private readonly IAppDbContext _context;

	public GetRedirectByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<RedirectDto>> Handle(GetRedirectByIdQuery request, CancellationToken ct)
	{
		var r = await _context.Redirects.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<RedirectDto>()
			.FirstOrDefaultAsync(ct);

		if (r is null) return Error.NotFound("Redirect.NotFound", "ریدایرکت یافت نشد.");
		return r;
	}
}