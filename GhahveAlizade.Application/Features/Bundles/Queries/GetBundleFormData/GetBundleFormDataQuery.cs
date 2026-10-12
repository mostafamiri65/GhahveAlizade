using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundleFormData;

public class BundleFormDataDto
{
	public List<LookupItemDto> Products { get; set; } = new();
}

public record GetBundleFormDataQuery : IRequest<ErrorOr<BundleFormDataDto>>;
