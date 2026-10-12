using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoryById;

public record GetBlogCategoryByIdQuery(long Id) : IRequest<ErrorOr<BlogCategoryDto>>;
