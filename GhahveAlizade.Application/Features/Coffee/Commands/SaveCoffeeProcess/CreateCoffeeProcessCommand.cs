using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

// Create
public record CreateCoffeeProcessCommand(
	string Name,
	string? Slug,
	string? Description,
	string? ImageUrl,
	string? IconUrl,
	bool IsActive,
	int DisplayOrder) : IRequest<ErrorOr<long>>;
