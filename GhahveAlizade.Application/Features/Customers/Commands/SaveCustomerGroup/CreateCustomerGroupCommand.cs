using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public record CreateCustomerGroupCommand(
	string Name, string? Description, decimal? DiscountPercent,
	long? PriceListId, bool IsActive, int DisplayOrder) : IRequest<ErrorOr<long>>;
