using FluentValidation;

namespace RadencyLibrary.CQRS.BookCq.Commands.RateBook
{
    public class RateBookCommandValidator : AbstractValidator<RateBookCommand>
    {
        public RateBookCommandValidator()
        {
            RuleFor(x => x.Score).Must((x) => x >= 1 && x <= 5);
        }
    }
}
