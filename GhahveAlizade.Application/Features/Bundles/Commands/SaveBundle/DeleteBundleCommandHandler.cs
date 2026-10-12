using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public class DeleteBundleCommandHandler
	: IRequestHandler<DeleteBundleCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteBundleCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBundleCommand request, CancellationToken ct)
	{
		var bundle = await _context.ProductBundles
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (bundle is null) return Error.NotFound("Bundle.NotFound", "بسته یافت نشد.");

		_context.ProductBundles.Remove(bundle);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
