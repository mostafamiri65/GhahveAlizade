using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetChatSessionsList;

public record GetChatSessionsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	Domain.Support.ChatSessionStatus? Status = null)
	: IRequest<ErrorOr<List<ChatSessionDto>>>;
