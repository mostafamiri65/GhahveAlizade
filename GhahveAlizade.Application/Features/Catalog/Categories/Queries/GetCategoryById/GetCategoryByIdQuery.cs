using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Categories.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(long Id) : IRequest<ErrorOr<CategoryDto>>;