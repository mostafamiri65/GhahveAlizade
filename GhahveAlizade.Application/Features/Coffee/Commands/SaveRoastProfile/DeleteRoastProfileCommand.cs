using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public record DeleteRoastProfileCommand(long Id) : IRequest<ErrorOr<Success>>;
