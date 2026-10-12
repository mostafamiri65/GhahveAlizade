using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostBySlug;

/// <summary>
/// نسخه عمومی GetBlogPostById — فقط پست‌های منتشرشده.
/// </summary>
public class GetBlogPostBySlugQueryHandler
	: IRequestHandler<GetBlogPostBySlugQuery, ErrorOr<BlogPostDto>>
{
	private readonly IAppDbContext _context;

	public GetBlogPostBySlugQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BlogPostDto>> Handle(GetBlogPostBySlugQuery request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.AsNoTracking()
			.Include(p => p.Category)
			.Include(p => p.PostTags).ThenInclude(pt => pt.BlogTag)
			.Where(p => p.Slug == request.Slug && !p.IsDeleted && p.IsPublished)
			.FirstOrDefaultAsync(ct);

		if (post is null)
			return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد یا دیگر در دسترس نیست.");

		return post.Adapt<BlogPostDto>();
	}
}