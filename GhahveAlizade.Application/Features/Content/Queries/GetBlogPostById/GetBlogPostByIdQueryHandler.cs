using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostById;

public class GetBlogPostByIdQueryHandler
	: IRequestHandler<GetBlogPostByIdQuery, ErrorOr<BlogPostDto>>
{
	private readonly IAppDbContext _context;

	public GetBlogPostByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BlogPostDto>> Handle(GetBlogPostByIdQuery request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.AsNoTracking()
			.Include(p => p.Category)
			.Include(p => p.PostTags).ThenInclude(pt => pt.BlogTag)
			.Where(p => p.Id == request.Id && !p.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (post is null) return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد.");
		return post.Adapt<BlogPostDto>();
	}
}