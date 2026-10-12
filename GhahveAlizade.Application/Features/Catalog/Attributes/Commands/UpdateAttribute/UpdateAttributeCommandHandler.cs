using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.UpdateAttribute;

public class UpdateAttributeCommandHandler
    : IRequestHandler<UpdateAttributeCommand, ErrorOr<Success>>
{
    private readonly IAppDbContext _context;

    public UpdateAttributeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<Success>> Handle(
        UpdateAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .Include(a => a.Values)
            .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

        if (attribute is null)
            return Error.NotFound("Attribute.NotFound", "ویژگی یافت نشد.");

        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, attribute.Id, cancellationToken);

        attribute.Name = request.Name.Trim();
        attribute.Slug = slug;
        attribute.Type = request.Type;
        attribute.Unit = request.Unit?.Trim();
        attribute.IsFilterable = request.IsFilterable;
        attribute.IsRequired = request.IsRequired;
        attribute.IsVariant = request.IsVariant;
        attribute.DisplayOrder = request.DisplayOrder;

        // ============ مقادیر ============
        // استراتژی: حذف همه و ساخت مجدد (به‌جز مواردی که ProductAttribute بهشون وابسته‌ست)
        var existingValueIds = attribute.Values.Select(v => v.Id).ToHashSet();
        var inputValueIds = request.Values.Where(v => v.Id.HasValue).Select(v => v.Id!.Value).ToHashSet();

        // حذف Valueهایی که در ورودی نیستن و به ProductAttribute وصل نیستن
        foreach (var v in attribute.Values.ToList())
        {
            if (!inputValueIds.Contains(v.Id))
            {
                var isUsed = await _context.ProductAttributes
                    .AnyAsync(pa => pa.AttributeValueId == v.Id && !pa.IsDeleted, cancellationToken);

                if (isUsed)
                {
                    // Soft delete نمی‌کنیم، فقط از لیست حذف نمی‌کنیم
                    // برای سادگی، فعلاً نادیده می‌گیریم
                    continue;
                }

                _context.AttributeValues.Remove(v);
            }
        }

        // آپدیت یا اضافه کردن
        foreach (var input in request.Values)
        {
            if (input.Id.HasValue && existingValueIds.Contains(input.Id.Value))
            {
                // آپدیت
                var existing = attribute.Values.First(v => v.Id == input.Id.Value);
                existing.Value = input.Value.Trim();
                existing.DisplayValue = input.DisplayValue?.Trim();
                existing.ColorCode = input.ColorCode?.Trim();
                existing.DisplayOrder = input.DisplayOrder;
            }
            else
            {
                // اضافه
                attribute.Values.Add(new AttributeValue
                {
                    Value = input.Value.Trim(),
                    DisplayValue = input.DisplayValue?.Trim(),
                    ColorCode = input.ColorCode?.Trim(),
                    DisplayOrder = input.DisplayOrder
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string baseSlug, long excludeId, CancellationToken ct)
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