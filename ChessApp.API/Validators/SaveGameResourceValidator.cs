using FluentValidation;
using ChessApp.API.Resources;

namespace ChessApp.API.Validators
{
    public class SaveGameResourceValidator : AbstractValidator<SaveGameResource>
    {
        public SaveGameResourceValidator()
        {
            const int maxLength = 50;
            const string errorMsg = "'Player Id' must be greater than 0.";

            RuleFor(m => m.Result).NotEmpty().MaximumLength(maxLength);

            RuleFor(m => m.PlayerId).NotEmpty().WithMessage(errorMsg);
        }
    }
}