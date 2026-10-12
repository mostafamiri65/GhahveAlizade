using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoMetadata;

public class DeleteSeoMetadataCommandHandler
	: IRequestHandler<DeleteSeoMetadataCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteSeoMetadataCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteSeoMetadataCommand request, CancellationToken ct)
	{
		var m = await _context.SeoMetadata
			.FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, ct);
		if (m is null) return Error.NotFound("Seo.NotFound", "متادیتا یافت نشد.");

		_context.SeoMetadata.Remove(m);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}