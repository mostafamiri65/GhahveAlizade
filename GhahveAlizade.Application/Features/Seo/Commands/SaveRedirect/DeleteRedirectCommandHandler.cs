using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public class DeleteRedirectCommandHandler
	: IRequestHandler<DeleteRedirectCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteRedirectCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteRedirectCommand request, CancellationToken ct)
	{
		var r = await _context.Redirects
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

		if (r is null) return Error.NotFound("Redirect.NotFound", "ریدایرکت یافت نشد.");

		_context.Redirects.Remove(r);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
