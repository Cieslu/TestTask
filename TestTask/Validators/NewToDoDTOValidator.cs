using FluentValidation;
using TestTask.ModelsDTO;

namespace TestTask.Validators
{
    public class NewToDoDTOValidator : AbstractValidator<NewToDoDTO>
    {
        public NewToDoDTOValidator()
        {
            RuleFor(t => t.DateAndTimeOfExpiry)
                .NotEmpty()
                .Must(d => d >= DateTime.Now);//The date can not be from the past.

            RuleFor(t => t.Title)//The title has to have a maximum of 20 characters and can not be empty.
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(t => t.Description)
                .NotEmpty()
                .MaximumLength(100);//The description has to have a maximum of 100 characters and can not be empty.
        }
    }
}
