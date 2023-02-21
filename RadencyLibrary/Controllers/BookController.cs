using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RadencyLibrary.CQRS.BookCq.Commands.Delete;
using RadencyLibrary.CQRS.BookCq.Commands.ReviewBook;
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
            var responce = await Mediator.Send(query);
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
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
            var responce = await Mediator.Send(query);
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
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

            var responce = await Mediator.Send(new GetBookDetailsQuery(id));
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
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
            var responce = await Mediator.Send(new DeleteBookCommand(id, secret));
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
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
            var responce = await Mediator.Send(query);
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
        }

        /// <summary>
        /// Save a review for the book
        /// </summary>
        /// <responce code="200">Success</responce>
        /// <responce code="400">BadRequest</responce>
        [HttpPut]
        [Route("/api/books/{id}/review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReviewSave(int id, [FromBody] ReviewBookCommand query)
        {
            query.Id = id;
            var responce = await Mediator.Send(query);
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
        }

        /// <summary>
        /// Rate a book
        /// </summary>
        /// <responce code="200">Success</responce>
        /// <responce code="400">BadRequest</responce>
        [HttpPut]
        [Route("/api/books/{id}/rate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RateSave(int id, [FromBody] ReviewBookCommand query)
        {
            query.Id = id;
            var responce = await Mediator.Send(query);
            if (!responce.Validated)
            {
                return BadRequest(responce.Errors);
            }
            return Ok(responce.Result);
        }
    }
}
