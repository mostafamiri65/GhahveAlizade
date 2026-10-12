using FluentValidation;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Promotion.Commands.SaveCoupon;

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
	public CreateCouponCommandValidator()
	{
		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("کد تخفیف الزامی است.")
			.MinimumLength(3).WithMessage("کد تخفیف باید حداقل 3 کاراکتر باشد.")
			.MaximumLength(50);

		RuleFor(x => x.DiscountValue)
			.GreaterThan(0).WithMessage("مقدار تخفیف باید بیشتر از صفر باشد.");

		// ⭐ منطق شرطی: اگه درصدی بود، بین 0 تا 100
		RuleFor(x => x.DiscountValue)
			.InclusiveBetween(0.01m, 100m)
			.WithMessage("مقدار تخفیف درصدی باید بین 0.01 تا 100 باشد.")
			.When(x => x.DiscountType == DiscountType.Percentage);

		// مبلغ ثابت: هر مقداری
		RuleFor(x => x.DiscountValue)
			.GreaterThan(0)
			.When(x => x.DiscountType == DiscountType.FixedAmount);

		// تاریخ پایان باید بعد از شروع باشه
		RuleFor(x => x.ExpiresAtUtc)
			.GreaterThan(x => x.StartsAtUtc)
			.WithMessage("تاریخ پایان باید بعد از تاریخ شروع باشد.")
			.When(x => x.StartsAtUtc.HasValue && x.ExpiresAtUtc.HasValue);
	}
}