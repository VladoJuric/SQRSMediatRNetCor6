using Application.Administrator.Commands;
using FluentValidation;

namespace Application.WebUI.Validators
{
    public class SaveUserValidator : AbstractValidator<SaveUserCommand>
    {
        public SaveUserValidator()
        {
            RuleFor(e => e.Name).NotNull()
                .WithMessage("Name is required.");

            RuleFor(e => e.Surname).NotEmpty()
                .WithMessage("Surname is required");
        }
    }
}
