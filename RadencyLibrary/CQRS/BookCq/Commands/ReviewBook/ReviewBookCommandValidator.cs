using FluentValidation;

namespace RadencyLibrary.CQRS.BookCq.Commands.ReviewBook
{
    public class ReviewBookCommandValidator : AbstractValidator<ReviewBookCommand>
    {
        public ReviewBookCommandValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.Reviewer).NotEmpty();
        }
    }
}
