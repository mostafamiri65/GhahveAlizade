using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public class ToggleSubscriberActiveCommandHandler
	: IRequestHandler<ToggleSubscriberActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleSubscriberActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleSubscriberActiveCommand request, CancellationToken ct)
	{
		var s = await _context.NewsletterSubscribers
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (s is null) return Error.NotFound("Subscriber.NotFound", "مشترک یافت نشد.");

		s.IsActive = !s.IsActive;
		if (!s.IsActive)
			s.UnsubscribedAtUtc = DateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return s.IsActive;
	}
}
