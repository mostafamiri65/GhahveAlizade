using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogTagById;

public record GetBlogTagByIdQuery(long Id) : IRequest<ErrorOr<BlogTagDto>>;

public class GetBlogTagByIdQueryHandler
	: IRequestHandler<GetBlogTagByIdQuery, ErrorOr<BlogTagDto>>
{
	private readonly IAppDbContext _context;
	public GetBlogTagByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BlogTagDto>> Handle(GetBlogTagByIdQuery request, CancellationToken ct)
	{
		var t = await _context.BlogTags
			.AsNoTracking()
			.Include(x => x.PostTags)
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (t is null) return Error.NotFound("BlogTag.NotFound", "برچسب یافت نشد.");
		return t.Adapt<BlogTagDto>();
	}
}