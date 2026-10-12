using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeOriginById;

public class GetCoffeeOriginByIdQueryHandler
    : IRequestHandler<GetCoffeeOriginByIdQuery, ErrorOr<CoffeeOriginDto>>
{
    private readonly IAppDbContext _context;

    public GetCoffeeOriginByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<CoffeeOriginDto>> Handle(
        GetCoffeeOriginByIdQuery request,
        CancellationToken cancellationToken)
    {
        var origin = await _context.CoffeeOrigins
            .AsNoTracking()
            .Include(o => o.Regions)
            .Where(o => o.Id == request.Id && !o.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (origin is null)
            return Error.NotFound("CoffeeOrigin.NotFound", "خاستگاه یافت نشد.");

        return origin.Adapt<CoffeeOriginDto>();
    }
}