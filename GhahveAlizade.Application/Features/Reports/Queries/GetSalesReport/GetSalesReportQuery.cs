using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetSalesReport;

public record GetSalesReportQuery(
	DateTime? FromDate = null,
	DateTime? ToDate = null) : IRequest<ErrorOr<SalesReportDto>>;
