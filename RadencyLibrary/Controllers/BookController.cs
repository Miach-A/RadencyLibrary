using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadencyLibrary.CQRS.BookCq.Commands.Delete;
using RadencyLibrary.CQRS.BookCq.Commands.Save;
using RadencyLibrary.CQRS.BookCq.Dto;
using RadencyLibrary.CQRS.BookCq.Queries.GetAll;
using RadencyLibrary.CQRS.BookCq.Queries.GetDetails;
using RadencyLibrary.CQRS.BookCq.Queries.GetRecommended;

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
        /// <responce code="400">BadRequest</responce>
        [HttpGet]
        [Route("/api/recommended")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
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
        /// <responce code="400">BadRequest</responce>
        [HttpGet]
        [Route("/api/books/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BookDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookDetails(int id)
        {
            try
            {
                var res = await Mediator.Send(new GetBookDetailsQuery(id));
                if (res == null)
                {
                    return BadRequest(new ValidationFailure("id", "id does not exist"));
                }
                return Ok(res);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        /// <summary>
        /// Delete a book using a secret key
        /// </summary>
        /// <responce code="200">Success</responce>
        /// <responce code="400">BadRequest</responce>
        [HttpDelete]
        [Route("/api/books/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookDelete(int id, [FromQuery] string secret)
        {
            try
            {
                await Mediator.Send(new DeleteBookCommand(id, secret));
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new ValidationFailure("id", ex.Message));
            }
        }


        /// <summary>
        /// Save a new book
        /// </summary>
        /// <responce code="200">Success</responce>
        /// <responce code="400">BadRequest</responce>
        [HttpPost]
        [Route("/api/books/save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookSave([FromBody] SaveBookCommand query)
        {
            try
            {
                await Mediator.Send(query);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new ValidationFailure("id", ex.Message));
            }
        }
    }
}
