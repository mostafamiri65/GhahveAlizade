using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	private readonly IAppDbContext _context;

	public UpdateProductCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام محصول الزامی است.")
			.MaximumLength(500);

		RuleFor(x => x.ShortDescription).MaximumLength(1000);

		RuleFor(x => x.Sku)
			.MaximumLength(100)
			.MustAsync(BeUniqueSkuAsync).WithMessage("این SKU قبلاً استفاده شده است.")
			.When(x => !string.IsNullOrWhiteSpace(x.Sku));

		RuleFor(x => x.BasePrice)
			.GreaterThanOrEqualTo(0).When(x => x.BasePrice.HasValue);

		RuleFor(x => x.BasePrice)
			.NotNull().WithMessage("برای محصول بدون واریانت، قیمت پایه الزامی است.")
			.When(x => !x.HasVariants);

		RuleFor(x => x.Variants)
			.NotEmpty().WithMessage("حداقل یک واریانت باید تعریف شود.")
			.When(x => x.HasVariants);

		RuleForEach(x => x.Variants)
			.ChildRules(v =>
			{
				v.RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
				v.RuleFor(x => x.Price).GreaterThan(0);
			});

		RuleFor(x => x)
			.MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
			.WithName("Slug");

		RuleFor(x => x.Id)
			.MustAsync(ProductExistsAsync).WithMessage("محصول یافت نشد.");
	}

	private async Task<bool> BeUniqueSkuAsync(UpdateProductCommand cmd, string? sku, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(sku)) return true;
		return !await _context.Products.AnyAsync(
			p => p.Sku == sku && p.Id != cmd.Id && !p.IsDeleted, ct);
	}

	private async Task<bool> BeUniqueSlugAsync(UpdateProductCommand cmd, CancellationToken ct)
	{
		var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
		return !await _context.Products.AnyAsync(
			p => p.Slug == slug && p.Id != cmd.Id && !p.IsDeleted, ct);
	}

	private async Task<bool> ProductExistsAsync(long id, CancellationToken ct)
		=> await _context.Products.AnyAsync(p => p.Id == id && !p.IsDeleted, ct);
}