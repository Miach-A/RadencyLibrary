using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RadencyLibrary.CQRS.Book.Dto;
using RadencyLibrary.CQRS.Book.Queries.GetAllBooks;
using RadencyLibrary.CQRS.Book.Queries.GetDetails;
using RadencyLibrary.CQRS.Book.Queries.GetRecommended;

namespace RadencyLibrary.Controllers
{
    public class BookController : ApiControllerBase
    {
        public BookController(ISender mediator) : base(mediator) { }

        /// <summary>
        /// Get all books. Order by provided value (title or author)
        /// </summary>
        /// <responce code="200">Success</responce>
        /// <responce code="400">BadRequest</responce>

        [HttpGet]
        [Route("/api/books")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Books([FromQuery] GetAllBookQuery query)
        {
            try
            {
                return Ok(await Mediator.Send(query));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        /// <summary>
        /// Get top 10 books with high rating and number of reviews greater than 10. You can filter books by specifying genre. Order by rating
        /// </summary>
        /// <responce code="200">Success</responce>
        [HttpGet]
        [Route("/api/recommended")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Recommended([FromQuery] GetRecommendedBookQuery query)
        {
            try
            {
                return Ok(await Mediator.Send(query));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        /// <summary>
        /// Get book details with the list of reviews
        /// </summary>
        /// <responce code="200">Success</responce>
        [HttpGet]
        [Route("/api/books/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BookDetails(int id)
        {
            try
            {
                var res = await Mediator.Send(new GetBookDetailsQuery(id));
                if (res == null)
                {
                    return BadRequest("id does not exist");
                }
                return Ok(res);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
