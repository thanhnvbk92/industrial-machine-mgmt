using Microsoft.AspNetCore.Mvc;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Services;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IMachineService _machineService;
    private readonly ILogger<MachinesController> _logger;

    public MachinesController(IMachineService machineService, ILogger<MachinesController> logger)
    {
        _machineService = machineService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
    {
        try
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving machines");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Machine>> GetMachine(int id)
    {
        try
        {
            var machine = await _machineService.GetMachineByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return Ok(machine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving machine {MachineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Machine>> CreateMachine(Machine machine)
    {
        try
        {
            var createdMachine = await _machineService.CreateMachineAsync(machine);
            return CreatedAtAction(nameof(GetMachine), new { id = createdMachine.Id }, createdMachine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating machine");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMachine(int id, Machine machine)
    {
        if (id != machine.Id)
        {
            return BadRequest("Machine ID mismatch");
        }

        try
        {
            await _machineService.UpdateMachineAsync(machine);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating machine {MachineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        try
        {
            await _machineService.DeleteMachineAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting machine {MachineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("station/{stationId}")]
    public async Task<ActionResult<IEnumerable<Machine>>> GetMachinesByStation(int stationId)
    {
        try
        {
            var machines = await _machineService.GetMachinesByStationAsync(stationId);
            return Ok(machines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving machines for station {StationId}", stationId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<Machine>> UpdateMachineStatus(int id, [FromBody] MachineStatus status)
    {
        try
        {
            var machine = await _machineService.UpdateMachineStatusAsync(id, status);
            return Ok(machine);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating machine status {MachineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}