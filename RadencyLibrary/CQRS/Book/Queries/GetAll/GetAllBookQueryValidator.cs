using FluentValidation;

namespace RadencyLibrary.CQRS.Book.Queries.GetAllBooks
{
    public class GetAllBookQueryValidator : AbstractValidator<GetAllBookQuery>
    {
        public GetAllBookQueryValidator()
        {
            When(x => x.Order != null, () =>
            {
                RuleFor(x => x.Order).IsEnumName(typeof(Order), false);
            });

        }
    }

    public enum Order
    {
        title,
        author
    }
}
