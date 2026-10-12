using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.ToggleProductActive;

public record ToggleProductActiveCommand(long Id) : IRequest<ErrorOr<bool>>;