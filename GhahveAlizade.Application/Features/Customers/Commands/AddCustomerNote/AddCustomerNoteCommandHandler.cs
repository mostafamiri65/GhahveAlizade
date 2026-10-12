using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.AddCustomerNote;

public class AddCustomerNoteCommandHandler
	: IRequestHandler<AddCustomerNoteCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public AddCustomerNoteCommandHandler(
		IAppDbContext context,
		ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<long>> Handle(AddCustomerNoteCommand request, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(request.Content))
			return Error.Validation("Note.Empty", "متن یادداشت الزامی است.");

		var customer = await _context.Customers
			.FirstOrDefaultAsync(c => c.Id == request.CustomerId && !c.IsDeleted, ct);

		if (customer is null)
			return Error.NotFound("Customer.NotFound", "مشتری یافت نشد.");

		var note = new Domain.Customer.CustomerNote
		{
			CustomerId = customer.Id,
			Content = request.Content.Trim(),
			Category = request.Category?.Trim(),
			IsInternal = request.IsInternal,
			CreatedAtUtc = DateTime.UtcNow,
			CreatedBy = _currentUser.UserId
		};

		_context.CustomerNotes.Add(note);
		await _context.SaveChangesAsync(ct);
		return note.Id;
	}
}