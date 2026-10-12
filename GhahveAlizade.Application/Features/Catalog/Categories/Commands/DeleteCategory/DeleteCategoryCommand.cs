using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(long Id) : IRequest<ErrorOr<Success>>;