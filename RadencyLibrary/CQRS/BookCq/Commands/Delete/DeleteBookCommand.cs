using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Commands.Delete
{
    public record DeleteBookCommand : IRequest
    {
        public DeleteBookCommand(int id, string secret)
        {
            Id = id;
            Secret = secret;
        }
        public int Id { get; set; }
        public string Secret { get; set; } = string.Empty;
    }

    public class GetAllBookQueryHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly LibraryDbContext _context;
        private readonly IConfiguration _configuration;
        public GetAllBookQueryHandler(
            LibraryDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Secret != _configuration.GetValue<string>("BooksApi:Secret"))
            {
                throw new ValidationException("secret incorrect",
                    new List<ValidationFailure>() {
                        new ValidationFailure("secret", "secret incorrect")
                    });
            }

            var book = _context.Books.Attach(new Book { Id = request.Id });
            book.State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return;
        }
    }
}
