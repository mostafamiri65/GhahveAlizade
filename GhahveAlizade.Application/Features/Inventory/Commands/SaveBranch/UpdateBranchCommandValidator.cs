using FluentValidation;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
	private readonly IAppDbContext _context;

	public UpdateBranchCommandValidator(IAppDbContext context)
	{
		_context = context;

		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("نام شعبه الزامی است.")
			.MaximumLength(200);

		RuleFor(x => x.Code)
			.NotEmpty().WithMessage("کد شعبه الزامی است.")
			.MaximumLength(50)
			.MustAsync(BeUniqueCodeAsync).WithMessage("این کد شعبه قبلاً استفاده شده است.");

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue);

		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue);
	}

	private async Task<bool> BeUniqueCodeAsync(UpdateBranchCommand cmd, string code, CancellationToken ct)
		=> !await _context.Branches.AnyAsync(
			b => b.Code == code && b.Id != cmd.Id && !b.IsDeleted, ct);
}