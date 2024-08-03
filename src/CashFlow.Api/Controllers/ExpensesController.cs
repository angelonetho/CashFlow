using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] RequestExpenseJson request)
        {
            try
            {
                var useCase = new RegisterExpenseUseCase();

                var response = useCase.Execute(request);

                return Created(string.Empty, response);
            }
            catch (ArgumentException exception)
            {
                var errorResponse = new ResponseErrorJson(exception.Message);

                return BadRequest(errorResponse);
            }
            catch
            {
                var errorResponse = new ResponseErrorJson("unknown error");
                
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse );
            }
        }
    }
}
