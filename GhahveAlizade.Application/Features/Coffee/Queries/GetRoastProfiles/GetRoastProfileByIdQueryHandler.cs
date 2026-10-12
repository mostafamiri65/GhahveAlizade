using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetRoastProfiles;

public class GetRoastProfileByIdQueryHandler
	: IRequestHandler<GetRoastProfileByIdQuery, ErrorOr<RoastProfileDto>>
{
	private readonly IAppDbContext _context;
	public GetRoastProfileByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<RoastProfileDto>> Handle(GetRoastProfileByIdQuery request, CancellationToken ct)
	{
		var r = await _context.RoastProfiles.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<RoastProfileDto>()
			.FirstOrDefaultAsync(ct);

		if (r is null)
			return Error.NotFound("RoastProfile.NotFound", "پروفایل رست یافت نشد.");

		return r;
	}
}