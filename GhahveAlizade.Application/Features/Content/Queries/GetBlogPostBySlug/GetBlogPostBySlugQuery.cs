using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostBySlug;

public record GetBlogPostBySlugQuery(string Slug) : IRequest<ErrorOr<BlogPostDto>>;