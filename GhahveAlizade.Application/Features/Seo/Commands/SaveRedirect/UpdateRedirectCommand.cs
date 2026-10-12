using ErrorOr;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public record UpdateRedirectCommand(
	long Id,
	string FromPath,
	string ToPath,
	RedirectType Type,
	bool IsActive,
	string? Notes) : IRequest<ErrorOr<Success>>;
