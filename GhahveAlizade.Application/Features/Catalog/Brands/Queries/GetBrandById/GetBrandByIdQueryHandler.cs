using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler
    : IRequestHandler<GetBrandByIdQuery, ErrorOr<BrandDto>>
{
    private readonly IAppDbContext _context;

    public GetBrandByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<BrandDto>> Handle(
        GetBrandByIdQuery request,
        CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .AsNoTracking()
            .Where(b => b.Id == request.Id && !b.IsDeleted)
            .ProjectToType<BrandDto>()
            .FirstOrDefaultAsync(cancellationToken);

        if (brand is null)
            return Error.NotFound("Brand.NotFound", "برند یافت نشد.");

        return brand;
    }
}