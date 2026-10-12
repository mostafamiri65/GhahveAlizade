using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public record UpdateBranchCommand(
	long Id,
	string Name,
	string Code,
	string? Address,
	string? City,
	string? Province,
	string? PostalCode,
	string? Phone,
	decimal? Latitude,
	decimal? Longitude,
	string? WorkingHours,
	bool IsActive,
	bool IsDefault,
	int DisplayOrder) : IRequest<ErrorOr<Success>>;
