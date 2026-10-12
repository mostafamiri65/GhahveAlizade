using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostById;

public record GetBlogPostByIdQuery(long Id) : IRequest<ErrorOr<BlogPostDto>>;
