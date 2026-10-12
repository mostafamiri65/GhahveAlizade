using ErrorOr;
using GhahveAlizade.Application.Features.Coffee.Commands.CreateCoffeeOrigin;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.UpdateCoffeeOrigin;

public record UpdateCoffeeOriginCommand(
    long Id,
    string Name,
    string? Slug,
    string? Description,
    string? CountryCode,
    string? FlagUrl,
    string? ImageUrl,
    bool IsActive,
    int DisplayOrder,
    List<CoffeeRegionInput> Regions) : IRequest<ErrorOr<Success>>;