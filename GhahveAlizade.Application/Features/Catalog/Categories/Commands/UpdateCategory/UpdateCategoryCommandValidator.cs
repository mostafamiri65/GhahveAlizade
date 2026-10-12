using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
	private readonly IAppDbContext _context;

	public UpdateCategoryCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
			.MaximumLength(300);

		RuleFor(x => x.Description)
			.MaximumLength(2000);

		RuleFor(x => x)
			.MustAsync(BeUniqueSlugAsync)
			.WithMessage("این نامک قبلاً استفاده شده است.")
			.WithName("Slug");

		RuleFor(x => x.ParentId)
			.MustAsync(NotBeSelfOrDescendantAsync)
			.WithMessage("نمی‌توانید یک دسته را زیرمجموعه‌ی خودش یا فرزندانش کنید.");
	}

	private async Task<bool> BeUniqueSlugAsync(UpdateCategoryCommand cmd, CancellationToken ct)
	{
		var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
		return !await _context.Categories.AnyAsync(
			c => c.Slug == slug && c.Id != cmd.Id && !c.IsDeleted, ct);
	}

	private async Task<bool> NotBeSelfOrDescendantAsync(UpdateCategoryCommand cmd, long? parentId, CancellationToken ct)
	{
		if (!parentId.HasValue) return true;
		if (parentId.Value == cmd.Id) return false;

		// چک کن که parent، فرزند دسته‌ی فعلی نباشه (جلوگیری از چرخه)
		var current = parentId.Value;
		var guard = 0;

		while (guard++ < 20)
		{
			var parent = await _context.Categories
				.AsNoTracking()
				.Where(c => c.Id == current)
				.Select(c => new { c.Id, c.ParentId })
				.FirstOrDefaultAsync(ct);

			if (parent is null) return true;
			if (parent.Id == cmd.Id) return false;
			if (!parent.ParentId.HasValue) return true;

			current = parent.ParentId.Value;
		}

		return true;
	}
}