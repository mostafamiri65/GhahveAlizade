using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public record DeleteCustomerGroupCommand(long Id) : IRequest<ErrorOr<Success>>;