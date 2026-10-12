using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Content;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public class CreateBlogTagCommandHandler
	: IRequestHandler<CreateBlogTagCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateBlogTagCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateBlogTagCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogTags.AnyAsync(t => t.Slug == slug && !t.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var tag = new BlogTag
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim()
		};

		_context.BlogTags.Add(tag);
		await _context.SaveChangesAsync(ct);
		return tag.Id;
	}
}
