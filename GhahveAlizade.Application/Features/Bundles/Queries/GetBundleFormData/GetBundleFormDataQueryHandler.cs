using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundleFormData;

public class GetBundleFormDataQueryHandler
	: IRequestHandler<GetBundleFormDataQuery, ErrorOr<BundleFormDataDto>>
{
	private readonly IAppDbContext _context;
	public GetBundleFormDataQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BundleFormDataDto>> Handle(
		GetBundleFormDataQuery request, CancellationToken ct)
	{
		var dto = new BundleFormDataDto
		{
			Products = await _context.Products
				.Where(p => !p.IsDeleted && p.IsActive && p.IsPublished)
				.OrderBy(p => p.Name)
				.Select(p => new LookupItemDto { Id = p.Id, Name = p.Name })
				.ToListAsync(ct)
		};

		return dto;
	}
}