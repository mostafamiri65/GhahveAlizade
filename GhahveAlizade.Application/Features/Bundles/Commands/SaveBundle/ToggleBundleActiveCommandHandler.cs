using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public class ToggleBundleActiveCommandHandler
	: IRequestHandler<ToggleBundleActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleBundleActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleBundleActiveCommand request, CancellationToken ct)
	{
		var bundle = await _context.ProductBundles
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);
		if (bundle is null) return Error.NotFound("Bundle.NotFound", "بسته یافت نشد.");

		bundle.IsActive = !bundle.IsActive;
		await _context.SaveChangesAsync(ct);
		return bundle.IsActive;
	}
}