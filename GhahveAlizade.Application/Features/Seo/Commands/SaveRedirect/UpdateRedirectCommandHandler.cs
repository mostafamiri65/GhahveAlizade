using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public class UpdateRedirectCommandHandler
	: IRequestHandler<UpdateRedirectCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateRedirectCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateRedirectCommand request, CancellationToken ct)
	{
		var redirect = await _context.Redirects
			.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, ct);

		if (redirect is null) return Error.NotFound("Redirect.NotFound", "ریدایرکت یافت نشد.");

		var from = NormalizePath(request.FromPath);
		if (await _context.Redirects.AnyAsync(
			r => r.FromPath == from && r.Id != redirect.Id && !r.IsDeleted, ct))
			return Error.Conflict("Redirect.FromExists", "این مسیر قبلاً ریدایرکت شده است.");

		redirect.FromPath = from;
		redirect.ToPath = NormalizePath(request.ToPath);
		redirect.Type = request.Type;
		redirect.IsActive = request.IsActive;
		redirect.Notes = request.Notes?.Trim();

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}

	private static string NormalizePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path)) return "/";
		path = path.Trim();
		if (!path.StartsWith('/')) path = "/" + path;
		return path;
	}
}
