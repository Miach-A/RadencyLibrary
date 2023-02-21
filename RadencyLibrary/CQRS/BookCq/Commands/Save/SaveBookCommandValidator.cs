using FluentValidation;

namespace RadencyLibrary.CQRS.BookCq.Commands.Save
{
    public class SaveBookCommandValidator : AbstractValidator<SaveBookCommand>
    {
        public SaveBookCommandValidator()
        {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.Cover).NotEmpty();
            RuleFor(x => x.Genre).NotEmpty();
        }
    }
}
