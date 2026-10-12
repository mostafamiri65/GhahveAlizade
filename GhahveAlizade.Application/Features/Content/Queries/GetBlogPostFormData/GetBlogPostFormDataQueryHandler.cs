using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostFormData;

public class GetBlogPostFormDataQueryHandler
	: IRequestHandler<GetBlogPostFormDataQuery, ErrorOr<BlogPostFormDataDto>>
{
	private readonly IAppDbContext _context;

	public GetBlogPostFormDataQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BlogPostFormDataDto>> Handle(
		GetBlogPostFormDataQuery request, CancellationToken ct)
	{
		var dto = new BlogPostFormDataDto
		{
			Categories = await _context.BlogCategories
				.Where(c => !c.IsDeleted && c.IsActive)
				.OrderBy(c => c.DisplayOrder)
				.Select(c => new LookupItemDto { Id = c.Id, Name = c.Name })
				.ToListAsync(ct),

			Tags = await _context.BlogTags
				.Where(t => !t.IsDeleted)
				.OrderBy(t => t.Name)
				.Select(t => new LookupItemDto { Id = t.Id, Name = t.Name })
				.ToListAsync(ct)
		};

		return dto;
	}
}