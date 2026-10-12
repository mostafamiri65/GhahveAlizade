using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.DeleteAttribute;

public class DeleteAttributeCommandHandler
    : IRequestHandler<DeleteAttributeCommand, ErrorOr<Success>>
{
    private readonly IAppDbContext _context;

    public DeleteAttributeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<Success>> Handle(
        DeleteAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .Include(a => a.Values)
            .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

        if (attribute is null)
            return Error.NotFound("Attribute.NotFound", "ویژگی یافت نشد.");

        // چک: آیا در محصولات استفاده شده؟
        var isUsed = await _context.ProductAttributes
            .AnyAsync(pa => pa.AttributeId == attribute.Id && !pa.IsDeleted, cancellationToken);

        if (isUsed)
            return Error.Conflict(
                "Attribute.InUse",
                "این ویژگی در محصولات استفاده شده و قابل حذف نیست.");

        _context.Attributes.Remove(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}