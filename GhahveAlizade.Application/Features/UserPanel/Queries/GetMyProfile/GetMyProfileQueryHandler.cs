using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyProfile;

public class GetMyProfileQueryHandler
	: IRequestHandler<GetMyProfileQuery, ErrorOr<MyProfileDto>>
{
	private readonly IAppDbContext _context;

	public GetMyProfileQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<MyProfileDto>> Handle(GetMyProfileQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var customer = await _context.Customers
			.AsNoTracking()
			.Include(c => c.CustomerGroup)
			.Where(c => c.UserId == userIdStr && !c.IsDeleted)
			.FirstOrDefaultAsync(ct);

		var points = customer is null
			? null
			: await _context.CustomerPointAccounts
				.AsNoTracking()
				.Where(p => p.CustomerId == customer.Id && !p.IsDeleted)
				.FirstOrDefaultAsync(ct);

		return new MyProfileDto
		{
			CustomerId = customer?.Id,
			UserId = request.UserId,
			FirstName = customer?.FirstName,
			LastName = customer?.LastName,
			Email = customer?.Email,
			PhoneNumber = customer?.PhoneNumber,
			NationalCode = customer?.NationalCode,
			BirthDate = customer?.BirthDate,
			Gender = customer?.Gender,
			AvatarUrl = customer?.AvatarUrl,
			AcceptsMarketing = customer?.AcceptsMarketing ?? false,
			Tier = points?.Tier,
			CurrentPoints = points?.CurrentPoints ?? 0
		};
	}
}
