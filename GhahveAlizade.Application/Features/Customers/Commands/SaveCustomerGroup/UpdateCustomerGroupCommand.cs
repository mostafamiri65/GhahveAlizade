using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public record UpdateCustomerGroupCommand(
	long Id, string Name, string? Description, decimal? DiscountPercent,
	long? PriceListId, bool IsActive, int DisplayOrder) : IRequest<ErrorOr<Success>>;
