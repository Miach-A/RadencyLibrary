using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.Common.Models;
using RadencyLibrary.CQRS.Base;
using RadencyLibrary.CQRS.BookCq.Commands.Save;
using RadencyLibraryDomain.Entities;
using RadencyLibraryInfrastructure.Persistence;
using System.Text.Json.Serialization;

namespace RadencyLibrary.CQRS.BookCq.Commands.ReviewCommand
{
    public record ReviewBookCommand : IRequest<Response<SaveResult, ValidationFailure>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Reviewer { get; set; } = string.Empty;
    }

    public class ReviewBookCommandHandler : IRequestHandler<ReviewBookCommand, Response<SaveResult, ValidationFailure>>
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;
        private readonly Response<SaveResult, ValidationFailure> _response;
        private readonly ILogger<SaveBookCommand> _logger;
        public ReviewBookCommandHandler(
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
        public async Task<Response<SaveResult, ValidationFailure>> Handle(ReviewBookCommand request, CancellationToken cancellationToken)
        {
            var review = _mapper.Map<Review>(request);
            await _context.AddAsync(review);

            try
            {
                var count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    _response.Result = new SaveResult() { Id = review.Id };
                    return _response;
                }
                else
                {
                    UpdateError(string.Concat("Id = ", request.Id, " not save"), request);
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                UpdateError(ex.Message, request);
            }

            return _response;

        }

        private void UpdateError(string message, ReviewBookCommand request)
        {
            _response.Validated = false;
            _response.Errors.Add(new ValidationFailure("id", message));
            var requestName = request.GetType().Name;
            _logger.LogError("Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);
        }
    }
}
