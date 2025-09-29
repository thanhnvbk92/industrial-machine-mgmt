using Microsoft.EntityFrameworkCore;
using ManagerApp.Data;
using ManagerApp.Models;

namespace ManagerApp.Services;

public class LogService : ILogService
{
    private readonly ApplicationDbContext _context;

    public LogService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LogEntry>> GetLogsAsync(int? machineId = null, Models.LogLevel? level = null, DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 100)
    {
        var query = _context.LogEntries
            .Include(l => l.Machine)
            .AsQueryable();

        if (machineId.HasValue)
            query = query.Where(l => l.MachineId == machineId.Value);

        if (level.HasValue)
            query = query.Where(l => l.Level == level.Value);

        if (from.HasValue)
            query = query.Where(l => l.Timestamp >= from.Value);

        if (to.HasValue)
            query = query.Where(l => l.Timestamp <= to.Value);

        return await query
            .OrderByDescending(l => l.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<LogEntry> CreateLogEntryAsync(LogEntry logEntry)
    {
        logEntry.Timestamp = DateTime.UtcNow;
        
        _context.LogEntries.Add(logEntry);
        await _context.SaveChangesAsync();
        
        // Load the machine for the return value
        await _context.Entry(logEntry)
            .Reference(l => l.Machine)
            .LoadAsync();
            
        return logEntry;
    }

    public async Task<List<LogEntry>> GetMachineLogsAsync(int machineId, int count = 100)
    {
        return await _context.LogEntries
            .Where(l => l.MachineId == machineId)
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToListAsync();
    }

    public async Task<int> GetLogCountByLevelAsync(Models.LogLevel level, DateTime? from = null)
    {
        var query = _context.LogEntries.Where(l => l.Level == level);
        
        if (from.HasValue)
            query = query.Where(l => l.Timestamp >= from.Value);

        return await query.CountAsync();
    }
}

public class CommandService : ICommandService
{
    private readonly ApplicationDbContext _context;

    public CommandService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Command>> GetAllCommandsAsync()
    {
        return await _context.Commands
            .Include(c => c.Machine)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Command?> GetCommandByIdAsync(int id)
    {
        return await _context.Commands
            .Include(c => c.Machine)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Command> CreateCommandAsync(Command command)
    {
        command.CreatedAt = DateTime.UtcNow;
        command.Status = CommandStatus.Pending;
        
        _context.Commands.Add(command);
        await _context.SaveChangesAsync();
        
        // Load the machine for the return value
        await _context.Entry(command)
            .Reference(c => c.Machine)
            .LoadAsync();
            
        return command;
    }

    public async Task<bool> UpdateCommandStatusAsync(int commandId, CommandStatus status, string? result = null, string? errorMessage = null)
    {
        var command = await _context.Commands.FindAsync(commandId);
        if (command == null)
            return false;

        command.Status = status;
        
        if (status == CommandStatus.Running && command.ExecutedAt == null)
        {
            command.ExecutedAt = DateTime.UtcNow;
        }
        
        if (status == CommandStatus.Completed || status == CommandStatus.Failed)
        {
            command.CompletedAt = DateTime.UtcNow;
            command.Result = result;
            command.ErrorMessage = errorMessage;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Command>> GetPendingCommandsAsync()
    {
        return await _context.Commands
            .Include(c => c.Machine)
            .Where(c => c.Status == CommandStatus.Pending)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Command>> GetMachineCommandsAsync(int machineId)
    {
        return await _context.Commands
            .Where(c => c.MachineId == machineId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> CancelCommandAsync(int commandId)
    {
        var command = await _context.Commands.FindAsync(commandId);
        if (command == null || command.Status != CommandStatus.Pending)
            return false;

        command.Status = CommandStatus.Cancelled;
        command.CompletedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
}