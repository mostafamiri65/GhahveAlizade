using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public class PublishSeoLandingPageCommandHandler
	: IRequestHandler<PublishSeoLandingPageCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public PublishSeoLandingPageCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<bool>> Handle(PublishSeoLandingPageCommand request, CancellationToken ct)
	{
		var page = await _context.SeoLandingPages
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);
		if (page is null) return Error.NotFound("LandingPage.NotFound", "صفحه یافت نشد.");

		page.IsPublished = !page.IsPublished;
		if (page.IsPublished && page.PublishedAtUtc is null)
			page.PublishedAtUtc = _dateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return page.IsPublished;
	}
}