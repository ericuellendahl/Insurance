using FluentValidation;
using Insurance.Hiring.Application.DTOs;
using Insurance.Hiring.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Hiring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController(IContractService _contractService,
                                    IValidator<ContractPropostRequest> _validator) : ControllerBase
    {



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ContractResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateContract([FromBody] ContractPropostRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                var response = await _contractService.CreateContractAsync(request);

                return CreatedAtAction(nameof(CreateContract), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetContract(Guid id)
        {
            try
            {
                var contract = await _contractService.GetContractByIdAsync(id);

                if (contract is null)
                    return NotFound();

                return Ok(contract);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    }
}
