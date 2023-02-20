using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RadencyLibrary.CQRS.Book.Queries.GetAllBooks;

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
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBooks([FromQuery] GetAllBookQuery query)
        {
            try
            {
                return Ok(await Mediator.Send(query));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
