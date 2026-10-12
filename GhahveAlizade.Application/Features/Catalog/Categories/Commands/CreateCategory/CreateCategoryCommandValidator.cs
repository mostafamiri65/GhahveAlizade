using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	private readonly IAppDbContext _context;

	public CreateCategoryCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
			.MaximumLength(300).WithMessage("نام دسته‌بندی نمی‌تواند بیشتر از 300 کاراکتر باشد.");

		RuleFor(x => x.Description)
			.MaximumLength(2000).WithMessage("توضیحات نمی‌تواند بیشتر از 2000 کاراکتر باشد.");

		RuleFor(x => x.DisplayOrder)
			.GreaterThanOrEqualTo(0).WithMessage("ترتیب نمایش نمی‌تواند منفی باشد.");

		RuleFor(x => x.MetaTitle)
			.MaximumLength(200).WithMessage("عنوان سئو نمی‌تواند بیشتر از 200 کاراکتر باشد.");

		RuleFor(x => x.MetaDescription)
			.MaximumLength(500).WithMessage("توضیحات سئو نمی‌تواند بیشتر از 500 کاراکتر باشد.");

		// چک کردن یکتا بودن Slug
		RuleFor(x => x)
			.MustAsync(BeUniqueSlugAsync)
			.WithMessage("این نامک قبلاً استفاده شده است.")
			.WithName("Slug");

		// چک کردن وجود والد
		RuleFor(x => x.ParentId)
			.MustAsync(ExistIfProvidedAsync)
			.WithMessage("دسته‌بندی والد انتخاب‌شده یافت نشد.");
	}

	private async Task<bool> BeUniqueSlugAsync(CreateCategoryCommand cmd, CancellationToken ct)
	{
		var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
		return !await _context.Categories.AnyAsync(c => c.Slug == slug && !c.IsDeleted, ct);
	}

	private async Task<bool> ExistIfProvidedAsync(long? parentId, CancellationToken ct)
	{
		if (!parentId.HasValue) return true;
		return await _context.Categories.AnyAsync(c => c.Id == parentId.Value && !c.IsDeleted, ct);
	}
}