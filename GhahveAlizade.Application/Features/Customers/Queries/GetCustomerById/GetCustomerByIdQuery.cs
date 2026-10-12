using ErrorOr;
using GhahveAlizade.Application.Features.Customers.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(long Id) : IRequest<ErrorOr<CustomerDto>>;
