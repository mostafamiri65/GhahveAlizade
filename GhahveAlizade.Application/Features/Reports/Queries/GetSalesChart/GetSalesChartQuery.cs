using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetSalesChart;

public record GetSalesChartQuery(int Days = 30) : IRequest<ErrorOr<List<SalesChartPointDto>>>;
