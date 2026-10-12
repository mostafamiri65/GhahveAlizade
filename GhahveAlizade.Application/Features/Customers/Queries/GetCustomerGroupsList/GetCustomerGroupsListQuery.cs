using ErrorOr;
using GhahveAlizade.Application.Features.Customers.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerGroupsList;

public record GetCustomerGroupsListQuery : IRequest<ErrorOr<List<CustomerGroupDto>>>;
