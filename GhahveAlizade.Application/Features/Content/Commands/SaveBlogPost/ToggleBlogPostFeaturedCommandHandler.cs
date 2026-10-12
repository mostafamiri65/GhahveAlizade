using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public class ToggleBlogPostFeaturedCommandHandler
	: IRequestHandler<ToggleBlogPostFeaturedCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleBlogPostFeaturedCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleBlogPostFeaturedCommand request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (post is null) return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد.");

		post.IsFeatured = !post.IsFeatured;
		await _context.SaveChangesAsync(ct);
		return post.IsFeatured;
	}
}