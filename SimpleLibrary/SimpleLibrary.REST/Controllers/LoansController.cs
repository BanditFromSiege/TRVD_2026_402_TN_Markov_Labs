using Microsoft.AspNetCore.Mvc;
using SimpleLibrary.Application.Services;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.REST.Models;

namespace SimpleLibrary.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: api/loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanResponseModel>>> GetAll()
        {
            var loans = await _loanService.GetAllLoansAsync();
            var result = loans.Select(MapToResponse);
            return Ok(result);
        }

        // GET: api/loans/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanResponseModel>> GetById(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
                return NotFound();

            return Ok(MapToResponse(loan));
        }

        // POST: api/loans
        [HttpPost]
        public async Task<ActionResult<LoanResponseModel>> Create([FromBody] LoanCreateModel model)
        {
            try
            {
                var loan = new Loan
                {
                    UserId = model.UserId,
                    BookId = model.BookId,
                    DueDate = model.DueDate,
                    IssuedAt = DateTime.UtcNow
                };

                var created = await _loanService.CreateLoanAsync(loan);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponse(created));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/loans/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<LoanResponseModel>> Update(int id, [FromBody] LoanUpdateModel model)
        {
            var loan = new Loan
            {
                ReturnedAt = model.ReturnDate
            };

            var updated = await _loanService.UpdateLoanAsync(id, loan);
            if (updated == null)
                return NotFound();

            return Ok(MapToResponse(updated));
        }

        // DELETE: api/loans/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _loanService.DeleteLoanAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private static LoanResponseModel MapToResponse(Loan loan)
        {
            return new LoanResponseModel
            {
                Id = loan.Id,
                UserId = loan.UserId,
                BookId = loan.BookId,
                LoanDate = loan.IssuedAt,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnedAt
            };
        }
    }
}