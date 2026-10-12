using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.CreateAttribute;

public class CreateAttributeCommandValidator : AbstractValidator<CreateAttributeCommand>
{
    private readonly IAppDbContext _context;

    public CreateAttributeCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام ویژگی الزامی است.")
            .MaximumLength(200);

        RuleFor(x => x.Unit).MaximumLength(50);

        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");

        // اگه نوع Select یا MultiSelect یا Color هست، حداقل یک Value لازمه
        RuleFor(x => x.Values)
            .NotEmpty().WithMessage("برای این نوع ویژگی، حداقل یک مقدار لازم است.")
            .When(x => x.Type is AttributeType.Select
                    or AttributeType.MultiSelect
                    or AttributeType.Color);

        RuleForEach(x => x.Values).ChildRules(v =>
        {
            v.RuleFor(x => x.Value)
                .NotEmpty().WithMessage("مقدار نمی‌تواند خالی باشد.")
                .MaximumLength(500);
        });
    }

    private async Task<bool> BeUniqueSlugAsync(CreateAttributeCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.Attributes.AnyAsync(a => a.Slug == slug && !a.IsDeleted, ct);
    }
}