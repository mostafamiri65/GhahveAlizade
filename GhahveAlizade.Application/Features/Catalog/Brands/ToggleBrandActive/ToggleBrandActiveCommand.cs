using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.ToggleBrandActive;

public record ToggleBrandActiveCommand(long Id) : IRequest<ErrorOr<bool>>;