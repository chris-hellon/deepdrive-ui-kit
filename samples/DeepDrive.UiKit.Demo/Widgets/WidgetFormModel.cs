using DeepDrive.UiKit.Abstractions;
using FluentValidation;

namespace DeepDrive.UiKit.Demo.Widgets;

public sealed class WidgetFormModel : IFormModel<CreateWidgetCommand, UpdateWidgetCommand>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal PriceUsd { get; set; }
    public string? Description { get; set; }

    public CreateWidgetCommand ToCreateCommand() => new()
    {
        Name = Name.Trim(),
        Category = Category.Trim(),
        PriceUsd = PriceUsd,
        Description = Description?.Trim()
    };

    public UpdateWidgetCommand ToUpdateCommand() => new()
    {
        Id = Id,
        Name = Name.Trim(),
        Category = Category.Trim(),
        PriceUsd = PriceUsd,
        Description = Description?.Trim()
    };

    public static WidgetFormModel FromDto(WidgetDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Category = dto.Category,
        PriceUsd = dto.PriceUsd,
        Description = dto.Description
    };
}

public sealed class WidgetFormModelValidator : AbstractValidator<WidgetFormModel>
{
    public WidgetFormModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(120).WithMessage("Name cannot exceed 120 characters.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(60).WithMessage("Category cannot exceed 60 characters.");

        RuleFor(x => x.PriceUsd)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
