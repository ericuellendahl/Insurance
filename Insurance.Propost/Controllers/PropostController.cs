using FluentValidation;
using Insurance.Propost.Application.DTOs;
using Insurance.Propost.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Propost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropostController(IPropostService _propostService,
                               IValidator<CreatePropostRequest> _validatorCreate,
                               IValidator<ChangePropostStatusRequest> _validatorChange,
                               ILogger<PropostController> _logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropostResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreatePropost([FromBody] CreatePropostRequest request)
    {
        _logger.LogInformation("Received request to create proposal");

        var validationResult = await _validatorCreate.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        try
        {
            var response = await _propostService.CreatePropostAsync(request);

            _logger.LogInformation("Proposal created successfully with ID: {Id}", response.Id);
            return CreatedAtAction(nameof(GetPropost), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating proposal");
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PropostResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetAllPropost()
    {
        var propost = await _propostService.GetAllPropostsAsync();
        return Ok(propost);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetPropost(Guid id)
    {
        var propost = await _propostService.GetPropostaAsync(id);
        if (propost == null)
            return NotFound();

        return Ok(propost);
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropostResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangePropostStatusRequest request)
    {
        _logger.LogInformation("Received request to change status for proposal ID: {Id}", id);
        if (id != request.PropostId)
            return BadRequest(new { Error = "ID da URL não confere com o ID do body" });

        var validationResult = await _validatorChange.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        try
        {
            var response = await _propostService.ChangePropostStatusAsync(request);
            _logger.LogInformation("Status changed successfully for proposal ID: {Id}", response.Id);

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Proposal not found for ID: {Id}", id);
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing status for proposal ID: {Id}", id);
            return BadRequest(new { Error = ex.Message });
        }
    }
}
