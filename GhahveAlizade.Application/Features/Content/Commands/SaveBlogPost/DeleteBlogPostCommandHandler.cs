using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public class DeleteBlogPostCommandHandler
	: IRequestHandler<DeleteBlogPostCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteBlogPostCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBlogPostCommand request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (post is null) return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد.");

		_context.BlogPosts.Remove(post);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
