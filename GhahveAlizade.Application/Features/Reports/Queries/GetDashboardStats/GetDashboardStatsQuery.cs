using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<ErrorOr<DashboardStatsDto>>;
