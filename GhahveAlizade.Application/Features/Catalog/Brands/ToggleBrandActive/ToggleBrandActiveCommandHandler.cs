using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.ToggleBrandActive;

public class ToggleBrandActiveCommandHandler
    : IRequestHandler<ToggleBrandActiveCommand, ErrorOr<bool>>
{
    private readonly IAppDbContext _context;

    public ToggleBrandActiveCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<bool>> Handle(
        ToggleBrandActiveCommand request,
        CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, cancellationToken);

        if (brand is null)
            return Error.NotFound("Brand.NotFound", "برند یافت نشد.");

        brand.IsActive = !brand.IsActive;
        await _context.SaveChangesAsync(cancellationToken);

        return brand.IsActive;
    }
}