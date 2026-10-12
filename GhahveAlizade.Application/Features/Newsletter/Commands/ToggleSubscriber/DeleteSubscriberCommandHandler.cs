using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public class DeleteSubscriberCommandHandler
	: IRequestHandler<DeleteSubscriberCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteSubscriberCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteSubscriberCommand request, CancellationToken ct)
	{
		var s = await _context.NewsletterSubscribers
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (s is null) return Error.NotFound("Subscriber.NotFound", "مشترک یافت نشد.");

		_context.NewsletterSubscribers.Remove(s);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
