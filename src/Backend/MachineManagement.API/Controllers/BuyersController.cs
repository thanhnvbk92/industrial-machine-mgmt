using Microsoft.AspNetCore.Mvc;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuyersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BuyersController> _logger;

    public BuyersController(IUnitOfWork unitOfWork, ILogger<BuyersController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Buyer>>> GetBuyers()
    {
        try
        {
            var buyers = await _unitOfWork.Buyers.GetAllAsync();
            return Ok(buyers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving buyers");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("with-lines")]
    public async Task<ActionResult<IEnumerable<Buyer>>> GetBuyersWithLines()
    {
        try
        {
            var buyers = await _unitOfWork.Buyers.GetBuyersWithLinesAsync();
            return Ok(buyers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving buyers with lines");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Buyer>> GetBuyer(int id)
    {
        try
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return Ok(buyer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving buyer {BuyerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Buyer>> CreateBuyer(Buyer buyer)
    {
        try
        {
            var createdBuyer = await _unitOfWork.Buyers.AddAsync(buyer);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBuyer), new { id = createdBuyer.Id }, createdBuyer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating buyer");
            return StatusCode(500, "Internal server error");
        }
    }
}