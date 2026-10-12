using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler
    : IRequestHandler<DeleteBrandCommand, ErrorOr<Success>>
{
    private readonly IAppDbContext _context;

    public DeleteBrandCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<Success>> Handle(
        DeleteBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, cancellationToken);

        if (brand is null)
            return Error.NotFound("Brand.NotFound", "برند یافت نشد.");

        var hasProducts = await _context.Products
            .AnyAsync(p => p.BrandId == brand.Id && !p.IsDeleted, cancellationToken);

        if (hasProducts)
            return Error.Conflict(
                "Brand.HasProducts",
                "این برند دارای محصول است و قابل حذف نیست.");

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}