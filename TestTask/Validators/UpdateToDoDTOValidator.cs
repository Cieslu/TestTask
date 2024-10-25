using FluentValidation;
using TestTask.ModelsDTO;

namespace TestTask.Validators
{
    public class UpdateToDoDTOValidator : AbstractValidator<UpdateToDoDTO>
    {
        public UpdateToDoDTOValidator()
        {
            RuleFor(t => t.Title)
                .MaximumLength(20);//The title has to have a maximum of 20 characters and can not be empty.

            RuleFor(t => t.Description)
                .MaximumLength(100);//The description has to have a maximum of 100 characters and can not be empty.
        }
    }
}
