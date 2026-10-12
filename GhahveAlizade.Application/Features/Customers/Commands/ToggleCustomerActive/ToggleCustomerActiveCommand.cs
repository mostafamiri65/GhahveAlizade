using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.ToggleCustomerActive;

public record ToggleCustomerActiveCommand(long Id) : IRequest<ErrorOr<bool>>;
