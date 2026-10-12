using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetInventoryReport;

public record GetInventoryReportQuery : IRequest<ErrorOr<InventoryReportDto>>;
