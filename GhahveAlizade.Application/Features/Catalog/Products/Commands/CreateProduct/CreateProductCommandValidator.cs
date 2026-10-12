using FluentValidation;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
	private readonly IAppDbContext _context;

	public CreateProductCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام محصول الزامی است.")
			.MaximumLength(500);

		RuleFor(x => x.ShortDescription)
			.MaximumLength(1000);

		RuleFor(x => x.Sku)
			.MaximumLength(100)
			.MustAsync(BeUniqueSkuAsync).WithMessage("این SKU قبلاً استفاده شده است.")
			.When(x => !string.IsNullOrWhiteSpace(x.Sku));

		RuleFor(x => x.BasePrice)
			.GreaterThanOrEqualTo(0).WithMessage("قیمت نمی‌تواند منفی باشد.")
			.When(x => x.BasePrice.HasValue);

		RuleFor(x => x.CompareAtPrice)
			.GreaterThanOrEqualTo(0).When(x => x.CompareAtPrice.HasValue);

		RuleFor(x => x.CostPrice)
			.GreaterThanOrEqualTo(0).When(x => x.CostPrice.HasValue);

		RuleFor(x => x.TaxRate)
			.InclusiveBetween(0, 100).WithMessage("نرخ مالیات باید بین 0 و 100 باشد.")
			.When(x => x.TaxRate.HasValue);

		// اگه Variant نداره، BasePrice الزامیه
		RuleFor(x => x.BasePrice)
			.NotNull().WithMessage("برای محصول بدون واریانت، قیمت پایه الزامی است.")
			.When(x => !x.HasVariants);

		// اگه Variant داره، حداقل یکی باید باشه
		RuleFor(x => x.Variants)
			.NotEmpty().WithMessage("حداقل یک واریانت باید تعریف شود.")
			.When(x => x.HasVariants);

		// اعتبارسنجی Variantها
		RuleForEach(x => x.Variants)
			.ChildRules(v =>
			{
				v.RuleFor(x => x.Name)
					.NotEmpty().WithMessage("نام واریانت الزامی است.")
					.MaximumLength(500);

				v.RuleFor(x => x.Price)
					.GreaterThan(0).WithMessage("قیمت واریانت باید بیشتر از صفر باشد.");
			});

		// اعتبارسنجی Slug یکتا
		RuleFor(x => x)
			.MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
			.WithName("Slug");

		// وجود CategoryId
		RuleFor(x => x.CategoryId)
			.MustAsync(CategoryExistsAsync).WithMessage("دسته‌بندی انتخاب‌شده یافت نشد.")
			.When(x => x.CategoryId.HasValue);

		RuleFor(x => x.BrandId)
			.MustAsync(BrandExistsAsync).WithMessage("برند انتخاب‌شده یافت نشد.")
			.When(x => x.BrandId.HasValue);

		// اعتبارسنجی Images
		RuleForEach(x => x.Images)
			.ChildRules(img =>
			{
				img.RuleFor(x => x.ImageUrl)
					.NotEmpty().WithMessage("آدرس تصویر الزامی است.")
					.MaximumLength(1000);
			});

		// اعتبارسنجی Attributes
		RuleForEach(x => x.Attributes)
			.ChildRules(attr =>
			{
				attr.RuleFor(x => x.AttributeId)
					.GreaterThan(0).WithMessage("ویژگی نامعتبر است.");
			});
	}

	private async Task<bool> BeUniqueSkuAsync(string? sku, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(sku)) return true;
		return !await _context.Products.AnyAsync(p => p.Sku == sku && !p.IsDeleted, ct);
	}

	private async Task<bool> BeUniqueSlugAsync(CreateProductCommand cmd, CancellationToken ct)
	{
		var slug = Common.SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
		return !await _context.Products.AnyAsync(p => p.Slug == slug && !p.IsDeleted, ct);
	}

	private async Task<bool> CategoryExistsAsync(long? id, CancellationToken ct)
	{
		if (!id.HasValue) return true;
		return await _context.Categories.AnyAsync(c => c.Id == id.Value && !c.IsDeleted, ct);
	}

	private async Task<bool> BrandExistsAsync(long? id, CancellationToken ct)
	{
		if (!id.HasValue) return true;
		return await _context.Brands.AnyAsync(b => b.Id == id.Value && !b.IsDeleted, ct);
	}
}