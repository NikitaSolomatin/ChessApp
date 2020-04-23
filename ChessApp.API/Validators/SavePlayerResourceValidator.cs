using FluentValidation;
using ChessApp.API.Resources;

namespace ChessApp.API.Validators
{
    public class SavePlayerResourceValidator : AbstractValidator<SavePlayerResource>
    {
        public SavePlayerResourceValidator()
        {
            const int maxLength = 50;

            RuleFor(a => a.FullName).NotEmpty().MaximumLength(maxLength);
        }
    }
}