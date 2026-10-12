using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public record CreateBlogPostCommand(
	string Title, string? Slug, string? Summary, string Content,
	string? FeaturedImageUrl, string? FeaturedImageAlt,
	long? CategoryId, string? AuthorName,
	bool IsPublished, bool IsFeatured, bool AllowComments,
	string? MetaTitle, string? MetaDescription, string? MetaKeywords,
	string? CanonicalUrl, string? OgImage,
	List<long> TagIds) : IRequest<ErrorOr<long>>;
