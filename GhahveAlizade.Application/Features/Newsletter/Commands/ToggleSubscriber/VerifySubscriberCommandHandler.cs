using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public class VerifySubscriberCommandHandler
	: IRequestHandler<VerifySubscriberCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public VerifySubscriberCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(VerifySubscriberCommand request, CancellationToken ct)
	{
		var s = await _context.NewsletterSubscribers
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (s is null) return Error.NotFound("Subscriber.NotFound", "مشترک یافت نشد.");

		s.IsVerified = true;
		s.VerificationToken = null;
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}