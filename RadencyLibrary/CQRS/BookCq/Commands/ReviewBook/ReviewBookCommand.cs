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

namespace RadencyLibrary.CQRS.BookCq.Commands.ReviewBook
{
    public record ReviewBookCommand : IRequest<Response<SaveResult, ValidationFailure>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Reviewer { get; set; } = string.Empty;
    }

    public class ReviewBookCommandHandler : CommandHandler, IRequestHandler<ReviewBookCommand, Response<SaveResult, ValidationFailure>>
    {
        private readonly Response<SaveResult, ValidationFailure> _response;
        public ReviewBookCommandHandler(
            LibraryDbContext context,
            IMapper mapper,
            Response<SaveResult, ValidationFailure> response,
            ILogger<SaveBookCommand> logger) : base(context, logger, mapper)
        {
            _response = response;
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
                    UpdateError(string.Concat("Id = ", request.Id, " not save"), _response, request);
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                UpdateError(ex.Message, _response, request);
            }

            return _response;

        }
    }
}
