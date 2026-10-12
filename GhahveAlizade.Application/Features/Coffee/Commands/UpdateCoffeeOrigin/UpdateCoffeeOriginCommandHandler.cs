using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.UpdateCoffeeOrigin;

public class UpdateCoffeeOriginCommandHandler
    : IRequestHandler<UpdateCoffeeOriginCommand, ErrorOr<Success>>
{
    private readonly IAppDbContext _context;

    public UpdateCoffeeOriginCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<Success>> Handle(
        UpdateCoffeeOriginCommand request,
        CancellationToken cancellationToken)
    {
        var origin = await _context.CoffeeOrigins
            .Include(o => o.Regions)
            .FirstOrDefaultAsync(o => o.Id == request.Id && !o.IsDeleted, cancellationToken);

        if (origin is null)
            return Error.NotFound("CoffeeOrigin.NotFound", "خاستگاه یافت نشد.");

        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, origin.Id, cancellationToken);

        origin.Name = request.Name.Trim();
        origin.Slug = slug;
        origin.Description = request.Description?.Trim();
        origin.CountryCode = request.CountryCode?.Trim();
        origin.FlagUrl = request.FlagUrl?.Trim();
        origin.ImageUrl = request.ImageUrl?.Trim();
        origin.IsActive = request.IsActive;
        origin.DisplayOrder = request.DisplayOrder;

        // ============ Regions ============
        var existingRegionIds = origin.Regions.Select(r => r.Id).ToHashSet();
        var inputRegionIds = request.Regions.Where(r => r.Id.HasValue).Select(r => r.Id!.Value).ToHashSet();

        foreach (var r in origin.Regions.ToList())
        {
            if (!inputRegionIds.Contains(r.Id))
                _context.CoffeeRegions.Remove(r);
        }

        foreach (var input in request.Regions)
        {
            if (input.Id.HasValue && existingRegionIds.Contains(input.Id.Value))
            {
                var existing = origin.Regions.First(r => r.Id == input.Id.Value);
                existing.Name = input.Name.Trim();
                existing.Slug = SlugHelper.GenerateSlug(input.Slug ?? input.Name);
                existing.Description = input.Description?.Trim();
                existing.Altitude = input.Altitude?.Trim();
                existing.IsActive = input.IsActive;
                existing.DisplayOrder = input.DisplayOrder;
            }
            else
            {
                origin.Regions.Add(new CoffeeRegion
                {
                    Name = input.Name.Trim(),
                    Slug = SlugHelper.GenerateSlug(input.Slug ?? input.Name),
                    Description = input.Description?.Trim(),
                    Altitude = input.Altitude?.Trim(),
                    IsActive = input.IsActive,
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

        while (await _context.CoffeeOrigins.AnyAsync(
            o => o.Slug == slug && o.Id != excludeId && !o.IsDeleted, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}