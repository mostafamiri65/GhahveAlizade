using ErrorOr;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetTopProducts;

public record GetTopProductsQuery(int Count = 10, int Days = 30)
	: IRequest<ErrorOr<List<TopProductDto>>>;
