using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public record UpdateWarehouseCommand(
	long Id, string Name, string Code, string? Address, string? City, string? Province,
	string? PostalCode, string? Phone, string? ManagerName,
	bool IsActive, bool IsDefault) : IRequest<ErrorOr<Success>>;
