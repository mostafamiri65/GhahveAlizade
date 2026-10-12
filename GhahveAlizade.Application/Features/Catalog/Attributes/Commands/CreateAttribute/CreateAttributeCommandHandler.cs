using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.CreateAttribute;

public class CreateAttributeCommandHandler
    : IRequestHandler<CreateAttributeCommand, ErrorOr<long>>
{
    private readonly IAppDbContext _context;

    public CreateAttributeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<long>> Handle(
        CreateAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

        var attribute = new Domain.Catalog.Attribute
        {
            Name = request.Name.Trim(),
            Slug = slug,
            Type = request.Type,
            Unit = request.Unit?.Trim(),
            IsFilterable = request.IsFilterable,
            IsRequired = request.IsRequired,
            IsVariant = request.IsVariant,
            DisplayOrder = request.DisplayOrder
        };

        foreach (var v in request.Values)
        {
            attribute.Values.Add(new AttributeValue
            {
                Value = v.Value.Trim(),
                DisplayValue = v.DisplayValue?.Trim(),
                ColorCode = v.ColorCode?.Trim(),
                DisplayOrder = v.DisplayOrder
            });
        }

        _context.Attributes.Add(attribute);
        await _context.SaveChangesAsync(cancellationToken);

        return attribute.Id;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string baseSlug, long? excludeId, CancellationToken ct)
    {
        var slug = baseSlug;
        var counter = 1;

        while (await _context.Attributes.AnyAsync(
            a => a.Slug == slug && a.Id != excludeId && !a.IsDeleted, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}