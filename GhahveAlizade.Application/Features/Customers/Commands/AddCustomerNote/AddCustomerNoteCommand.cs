using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.AddCustomerNote;

public record AddCustomerNoteCommand(
	long CustomerId,
	string Content,
	string? Category,
	bool IsInternal) : IRequest<ErrorOr<long>>;
