using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
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
	string? MetaKeywords) : IRequest<ErrorOr<long>>;