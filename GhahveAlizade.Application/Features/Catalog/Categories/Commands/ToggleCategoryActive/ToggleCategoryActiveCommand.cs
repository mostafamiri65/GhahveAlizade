using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.ToggleCategoryActive;

public record ToggleCategoryActiveCommand(long Id) : IRequest<ErrorOr<bool>>;