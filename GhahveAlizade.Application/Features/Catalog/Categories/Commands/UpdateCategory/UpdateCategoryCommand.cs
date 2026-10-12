using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
	long Id,
	string Name,
	string? Slug,
	string? Description,
	string? ImageUrl,
	string? IconUrl,
	long? ParentId,
	int DisplayOrder,
	bool IsActive,
	bool IsFeatured,
	string? MetaTitle,
	string? MetaDescription,
	string? MetaKeywords) : IRequest<ErrorOr<Success>>;