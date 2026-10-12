using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

// Update
public record UpdateCoffeeProcessCommand(
	long Id,
	string Name,
	string? Slug,
	string? Description,
	string? ImageUrl,
	string? IconUrl,
	bool IsActive,
	int DisplayOrder) : IRequest<ErrorOr<Success>>;
