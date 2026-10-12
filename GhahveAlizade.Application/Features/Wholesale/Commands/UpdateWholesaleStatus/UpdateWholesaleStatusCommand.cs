using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Wholesale.Commands.UpdateWholesaleStatus;

public record UpdateWholesaleStatusCommand(
	long Id, WholesaleRequestStatus Status, string? ReviewNotes)
	: IRequest<ErrorOr<Success>>;

public class UpdateWholesaleStatusCommandHandler
	: IRequestHandler<UpdateWholesaleStatusCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly ICurrentUserService _currentUser;
	private readonly IDateTimeProvider _dateTime;

	public UpdateWholesaleStatusCommandHandler(
		IAppDbContext context,
		ICurrentUserService currentUser,
		IDateTimeProvider dateTime)
	{
		_context = context;
		_currentUser = currentUser;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(
		UpdateWholesaleStatusCommand request, CancellationToken ct)
	{
		var r = await _context.WholesaleRequests
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

		if (r is null) return Error.NotFound("Wholesale.NotFound", "درخواست یافت نشد.");

		r.Status = request.Status;
		r.ReviewedAtUtc = _dateTime.UtcNow;
		r.ReviewedBy = _currentUser.UserName;
		if (!string.IsNullOrWhiteSpace(request.ReviewNotes))
			r.ReviewNotes = request.ReviewNotes.Trim();

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}