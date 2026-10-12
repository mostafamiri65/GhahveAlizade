using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeProcesses;

public class GetCoffeeProcessByIdQueryHandler
	: IRequestHandler<GetCoffeeProcessByIdQuery, ErrorOr<CoffeeProcessDto>>
{
	private readonly IAppDbContext _context;
	public GetCoffeeProcessByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<CoffeeProcessDto>> Handle(GetCoffeeProcessByIdQuery request, CancellationToken ct)
	{
		var p = await _context.CoffeeProcesses
			.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<CoffeeProcessDto>()
			.FirstOrDefaultAsync(ct);

		if (p is null)
			return Error.NotFound("CoffeeProcess.NotFound", "فرآیند یافت نشد.");

		return p;
	}
}