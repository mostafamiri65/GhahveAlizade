using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetLowStockAlerts;

public record GetLowStockAlertsQuery(int Count = 10) : IRequest<ErrorOr<List<LowStockAlertDto>>>;
