using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public record UpdateBlogCategoryCommand(
	long Id, string Name, string? Slug, string? Description, string? ImageUrl,
	long? ParentId, int DisplayOrder, bool IsActive,
	string? MetaTitle, string? MetaDescription, string? CanonicalUrl)
	: IRequest<ErrorOr<Success>>;
