using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.CreateCoffeeOrigin;

public record CreateCoffeeOriginCommand(
    string Name,
    string? Slug,
    string? Description,
    string? CountryCode,
    string? FlagUrl,
    string? ImageUrl,
    bool IsActive,
    int DisplayOrder,
    List<CoffeeRegionInput> Regions) : IRequest<ErrorOr<long>>;

public record CoffeeRegionInput(
    long? Id,
    string Name,
    string? Slug,
    string? Description,
    string? Altitude,
    bool IsActive,
    int DisplayOrder);