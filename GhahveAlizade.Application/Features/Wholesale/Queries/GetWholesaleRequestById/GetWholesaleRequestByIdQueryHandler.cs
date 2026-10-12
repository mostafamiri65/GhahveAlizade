using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Wholesale.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestById;

public class GetWholesaleRequestByIdQueryHandler
	: IRequestHandler<GetWholesaleRequestByIdQuery, ErrorOr<WholesaleRequestDto>>
{
	private readonly IAppDbContext _context;
	public GetWholesaleRequestByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<WholesaleRequestDto>> Handle(
		GetWholesaleRequestByIdQuery request, CancellationToken ct)
	{
		var r = await _context.WholesaleRequests.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<WholesaleRequestDto>()
			.FirstOrDefaultAsync(ct);

		if (r is null) return Error.NotFound("Wholesale.NotFound", "درخواست یافت نشد.");
		return r;
	}
}