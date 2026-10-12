using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public class PublishBlogPostCommandHandler
	: IRequestHandler<PublishBlogPostCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public PublishBlogPostCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<bool>> Handle(PublishBlogPostCommand request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (post is null) return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد.");

		post.IsPublished = !post.IsPublished;
		if (post.IsPublished && post.PublishedAtUtc is null)
			post.PublishedAtUtc = _dateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return post.IsPublished;
	}
}
