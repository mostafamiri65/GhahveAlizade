using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public class DeleteSeoLandingPageCommandHandler
	: IRequestHandler<DeleteSeoLandingPageCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteSeoLandingPageCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteSeoLandingPageCommand request, CancellationToken ct)
	{
		var page = await _context.SeoLandingPages
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);
		if (page is null) return Error.NotFound("LandingPage.NotFound", "صفحه یافت نشد.");

		_context.SeoLandingPages.Remove(page);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
