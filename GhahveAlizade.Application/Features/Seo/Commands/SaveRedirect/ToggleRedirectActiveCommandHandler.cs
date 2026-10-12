using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public class ToggleRedirectActiveCommandHandler
	: IRequestHandler<ToggleRedirectActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleRedirectActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleRedirectActiveCommand request, CancellationToken ct)
	{
		var r = await _context.Redirects
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

		if (r is null) return Error.NotFound("Redirect.NotFound", "ریدایرکت یافت نشد.");

		r.IsActive = !r.IsActive;
		await _context.SaveChangesAsync(ct);
		return r.IsActive;
	}
}