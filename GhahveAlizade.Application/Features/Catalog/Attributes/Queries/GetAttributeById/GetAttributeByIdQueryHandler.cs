using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributeById;

public class GetAttributeByIdQueryHandler
    : IRequestHandler<GetAttributeByIdQuery, ErrorOr<AttributeDto>>
{
    private readonly IAppDbContext _context;

    public GetAttributeByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<AttributeDto>> Handle(
        GetAttributeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var attr = await _context.Attributes
            .AsNoTracking()
            .Include(a => a.Values)
            .Include(a => a.ProductAttributes)
            .Where(a => a.Id == request.Id && !a.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (attr is null)
            return Error.NotFound("Attribute.NotFound", "ویژگی یافت نشد.");

        return attr.Adapt<AttributeDto>();
    }
}