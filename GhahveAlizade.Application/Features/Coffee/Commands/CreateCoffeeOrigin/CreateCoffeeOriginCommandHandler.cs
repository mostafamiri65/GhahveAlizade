using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.CreateCoffeeOrigin;

public class CreateCoffeeOriginCommandHandler
    : IRequestHandler<CreateCoffeeOriginCommand, ErrorOr<long>>
{
    private readonly IAppDbContext _context;

    public CreateCoffeeOriginCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<long>> Handle(
        CreateCoffeeOriginCommand request,
        CancellationToken cancellationToken)
    {
        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

        var origin = new CoffeeOrigin
        {
            Name = request.Name.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            CountryCode = request.CountryCode?.Trim(),
            FlagUrl = request.FlagUrl?.Trim(),
            ImageUrl = request.ImageUrl?.Trim(),
            IsActive = request.IsActive,
            DisplayOrder = request.DisplayOrder
        };

        foreach (var r in request.Regions)
        {
            origin.Regions.Add(new CoffeeRegion
            {
                Name = r.Name.Trim(),
                Slug = SlugHelper.GenerateSlug(r.Slug ?? r.Name),
                Description = r.Description?.Trim(),
                Altitude = r.Altitude?.Trim(),
                IsActive = r.IsActive,
                DisplayOrder = r.DisplayOrder
            });
        }

        _context.CoffeeOrigins.Add(origin);
        await _context.SaveChangesAsync(cancellationToken);

        return origin.Id;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string baseSlug, long? excludeId, CancellationToken ct)
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