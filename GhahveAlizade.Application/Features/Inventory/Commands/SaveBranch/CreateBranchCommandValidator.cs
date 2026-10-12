using FluentValidation;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
	private readonly IAppDbContext _context;

	public CreateBranchCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام شعبه الزامی است.")
			.MaximumLength(200);

		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("کد شعبه الزامی است.")
			.MaximumLength(50)
			.MustAsync(BeUniqueCodeAsync).WithMessage("این کد شعبه قبلاً استفاده شده است.");

		RuleFor(x => x.Phone)
			.MaximumLength(20)
			.When(x => !string.IsNullOrWhiteSpace(x.Phone));

		RuleFor(x => x.PostalCode)
			.MaximumLength(20)
			.When(x => !string.IsNullOrWhiteSpace(x.PostalCode));

		RuleFor(x => x.DisplayOrder)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90).WithMessage("عرض جغرافیایی باید بین -90 و 90 باشد.")
			.When(x => x.Latitude.HasValue);

		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180).WithMessage("طول جغرافیایی باید بین -180 و 180 باشد.")
			.When(x => x.Longitude.HasValue);
	}

	private async Task<bool> BeUniqueCodeAsync(string code, CancellationToken ct)
		=> !await _context.Branches.AnyAsync(b => b.Code == code && !b.IsDeleted, ct);
}