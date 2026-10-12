using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Commerce;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetRecentOrders;

public record GetRecentOrdersQuery(int Count = 10) : IRequest<ErrorOr<List<RecentOrderDto>>>;
