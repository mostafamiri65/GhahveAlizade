using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.UpdateAttribute;

public class UpdateAttributeCommandValidator : AbstractValidator<UpdateAttributeCommand>
{
    private readonly IAppDbContext _context;

    public UpdateAttributeCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Unit).MaximumLength(50);

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");

        RuleFor(x => x.Values)
            .NotEmpty().WithMessage("برای این نوع ویژگی، حداقل یک مقدار لازم است.")
            .When(x => x.Type is AttributeType.Select
                    or AttributeType.MultiSelect
                    or AttributeType.Color);

        RuleForEach(x => x.Values).ChildRules(v =>
        {
            v.RuleFor(x => x.Value).NotEmpty().MaximumLength(500);
        });
    }

    private async Task<bool> BeUniqueSlugAsync(UpdateAttributeCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.Attributes.AnyAsync(
            a => a.Slug == slug && a.Id != cmd.Id && !a.IsDeleted, ct);
    }
}