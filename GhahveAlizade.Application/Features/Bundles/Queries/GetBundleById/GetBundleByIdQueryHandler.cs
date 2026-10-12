using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Bundles.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundleById;

public class GetBundleByIdQueryHandler
	: IRequestHandler<GetBundleByIdQuery, ErrorOr<ProductBundleDto>>
{
	private readonly IAppDbContext _context;
	public GetBundleByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ProductBundleDto>> Handle(GetBundleByIdQuery request, CancellationToken ct)
	{
		var bundle = await _context.ProductBundles
			.AsNoTracking()
			.Include(b => b.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Images)
			.Include(b => b.Items).ThenInclude(i => i.ProductVariant)
			.Where(b => b.Id == request.Id && !b.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (bundle is null) return Error.NotFound("Bundle.NotFound", "بسته یافت نشد.");
		return bundle.Adapt<ProductBundleDto>();
	}
}