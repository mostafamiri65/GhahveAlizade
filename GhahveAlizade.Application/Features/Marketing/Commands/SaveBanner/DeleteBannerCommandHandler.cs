using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public class DeleteBannerCommandHandler
	: IRequestHandler<DeleteBannerCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteBannerCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBannerCommand request, CancellationToken ct)
	{
		var banner = await _context.Banners
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (banner is null)
			return Error.NotFound("Banner.NotFound", "بنر یافت نشد.");

		_context.Banners.Remove(banner);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
