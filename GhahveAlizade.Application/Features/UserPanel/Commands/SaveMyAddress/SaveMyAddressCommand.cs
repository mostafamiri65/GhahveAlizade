using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.SaveMyAddress;

public record SaveMyAddressCommand(
	long UserId,
	long? AddressId, // null = جدید
	string Title,
	string FirstName,
	string LastName,
	string PhoneNumber,
	string Province,
	string City,
	string PostalCode,
	string AddressLine1,
	string? AddressLine2,
	bool IsDefault) : IRequest<ErrorOr<Created>>;
