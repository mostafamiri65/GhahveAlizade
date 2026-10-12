using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public class DeleteBlogTagCommandHandler
	: IRequestHandler<DeleteBlogTagCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteBlogTagCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBlogTagCommand request, CancellationToken ct)
	{
		var tag = await _context.BlogTags
			.FirstOrDefaultAsync(t => t.Id == request.Id && !t.IsDeleted, ct);

		if (tag is null) return Error.NotFound("BlogTag.NotFound", "برچسب یافت نشد.");

		_context.BlogTags.Remove(tag);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}