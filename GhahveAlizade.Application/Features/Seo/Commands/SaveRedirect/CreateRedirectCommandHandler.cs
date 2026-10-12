using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.SEO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public class CreateRedirectCommandHandler
	: IRequestHandler<CreateRedirectCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;

	public CreateRedirectCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateRedirectCommand request, CancellationToken ct)
	{
		var from = NormalizePath(request.FromPath);

		if (await _context.Redirects.AnyAsync(r => r.FromPath == from && !r.IsDeleted, ct))
			return Error.Conflict("Redirect.FromExists", "این مسیر قبلاً ریدایرکت شده است.");

		var redirect = new Redirect
		{
			FromPath = from,
			ToPath = NormalizePath(request.ToPath),
			Type = request.Type,
			IsActive = request.IsActive,
			Notes = request.Notes?.Trim()
		};

		_context.Redirects.Add(redirect);
		await _context.SaveChangesAsync(ct);
		return redirect.Id;
	}

	private static string NormalizePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path)) return "/";
		path = path.Trim();
		if (!path.StartsWith('/')) path = "/" + path;
		return path;
	}
}
