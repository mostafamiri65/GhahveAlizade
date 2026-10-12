using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public class UpdateBlogTagCommandHandler
	: IRequestHandler<UpdateBlogTagCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateBlogTagCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateBlogTagCommand request, CancellationToken ct)
	{
		var tag = await _context.BlogTags
			.FirstOrDefaultAsync(t => t.Id == request.Id && !t.IsDeleted, ct);

		if (tag is null) return Error.NotFound("BlogTag.NotFound", "برچسب یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogTags.AnyAsync(
			t => t.Slug == slug && t.Id != tag.Id && !t.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		tag.Name = request.Name.Trim();
		tag.Slug = slug;
		tag.Description = request.Description?.Trim();

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
