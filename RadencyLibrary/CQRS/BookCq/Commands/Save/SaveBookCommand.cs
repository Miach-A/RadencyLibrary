using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.Common.Models;
using RadencyLibrary.CQRS.Base;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.CQRS.BookCq.Commands.Save
{
    public record SaveBookCommand : IRequest<Response<SaveResult, ValidationFailure>>//SaveResult>
    {
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }

    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, Response<SaveResult, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<SaveResult, ValidationFailure> _response;
        private readonly ILogger<SaveBookCommand> _logger;
        public SaveBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<SaveResult, ValidationFailure> response,
            ILogger<SaveBookCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _response = response;
            _logger = logger;
        }
        public async Task<Response<SaveResult, ValidationFailure>> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            Book? book;

            if (request.Id != null)
            {
                book = _context.Books.FirstOrDefault(x => x.Id == request.Id);
                if (book == null)
                {
                    UpdateError(string.Concat("Id= ", request.Id, " not save"), request);
                    return _response;
                };
                _mapper.Map(request, book);
                _context.Update(book);
            }
            else
            {
                book = _mapper.Map<Book>(request);
                await _context.AddAsync(book);
            }

            try
            {
                var count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _response.Result = new SaveResult() { Id = book.Id };
                    return _response;
                }
                else
                {
                    UpdateError(string.Concat("Id= ", request.Id, " not save"), request);
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                UpdateError(ex.Message, request);
            }

            return _response;
        }

        private void UpdateError(string message, SaveBookCommand request)
        {
            _response.Validated = false;
            _response.Errors.Add(new ValidationFailure("id", message));
            var requestName = request.GetType().Name;
            _logger.LogError("Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);
        }
    }
}