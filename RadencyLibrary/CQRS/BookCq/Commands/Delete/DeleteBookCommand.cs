using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.Base;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Commands.Delete
{
    public record DeleteBookCommand : IRequest<Response<bool, ValidationFailure>>
    {
        public DeleteBookCommand(int id, string secret)
        {
            Id = id;
            Secret = secret;
        }
        public int Id { get; set; }
        public string Secret { get; set; } = string.Empty;
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Response<bool, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly Response<bool, ValidationFailure> _response;
        private readonly ILogger<DeleteBookCommand> _logger;
        public DeleteBookCommandHandler(
            LibraryDbContext context,
            IConfiguration configuration,
            Response<bool, ValidationFailure> response,
            ILogger<DeleteBookCommand> logger)
        {
            _context = context;
            _configuration = configuration;
            _response = response;
            _logger = logger;
        }
        public async Task<Response<bool, ValidationFailure>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request.Secret != _configuration.GetValue<string>("BooksApi:Secret"))
            {
                _response.Validated = false;
                _response.Errors.Add(new ValidationFailure("secret", "secret incorrect"));
                return _response;
            }

            var book = _context.Books.Attach(new Book { Id = request.Id });
            book.State = EntityState.Deleted;

            try
            {
                var count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _response.Result = true;
                }
                else
                {
                    DeleteError(string.Concat("Id= ", request.Id, " not deleted"), request);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                DeleteError(ex.Message, request);
            }

            return _response;
        }
        private void DeleteError(string message, DeleteBookCommand request)
        {
            _response.Validated = false;
            _response.Errors.Add(new ValidationFailure("id", message));
            var requestName = request.GetType().Name;

            _logger.LogError("Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);
        }
    }
}
