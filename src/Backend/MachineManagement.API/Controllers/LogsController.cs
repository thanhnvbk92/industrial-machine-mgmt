using Microsoft.AspNetCore.Mvc;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Services;

namespace MachineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;
    private readonly ILogger<LogsController> _logger;

    public LogsController(ILogService logService, ILogger<LogsController> logger)
    {
        _logService = logService;
        _logger = logger;
    }

    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<MachineLog>>> GetRecentLogs([FromQuery] int count = 100)
    {
        try
        {
            var logs = await _logService.GetRecentLogsAsync(count);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent logs");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("machine/{machineId}")]
    public async Task<ActionResult<IEnumerable<MachineLog>>> GetLogsByMachine(int machineId)
    {
        try
        {
            var logs = await _logService.GetLogsByMachineAsync(machineId);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving logs for machine {MachineId}", machineId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<MachineLog>>> GetLogsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var logs = await _logService.GetLogsByDateRangeAsync(startDate, endDate);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving logs for date range");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<MachineLog>> AddLog(MachineLog log)
    {
        try
        {
            var createdLog = await _logService.AddLogAsync(log);
            return CreatedAtAction(nameof(GetLogsByMachine), new { machineId = log.MachineId }, createdLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding log");
            return StatusCode(500, "Internal server error");
        }
    }
}