using ErrorOr;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public record CreateCouponCommand(
	string Code,
	string? Description,
	DiscountType DiscountType,
	decimal DiscountValue,
	decimal? MaxDiscountAmount,
	decimal? MinOrderAmount,
	int? MaxUsageCount,
	int? MaxUsagePerCustomer,
	DateTime? StartsAtUtc,
	DateTime? ExpiresAtUtc,
	bool IsActive,
	bool IsFirstOrderOnly,
	bool IsPublic) : IRequest<ErrorOr<long>>;
