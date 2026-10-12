using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetTopCustomers;

public record GetTopCustomersQuery(int Count = 10) : IRequest<ErrorOr<List<TopCustomerDto>>>;
