using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.DeleteCustomerNote;

public record DeleteCustomerNoteCommand(long Id) : IRequest<ErrorOr<Success>>;
