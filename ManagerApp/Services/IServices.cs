using ManagerApp.Models;

namespace ManagerApp.Services;

public interface IMachineService
{
    Task<List<Machine>> GetAllMachinesAsync();
    Task<Machine?> GetMachineByIdAsync(int id);
    Task<Machine> CreateMachineAsync(Machine machine);
    Task<Machine> UpdateMachineAsync(Machine machine);
    Task<bool> DeleteMachineAsync(int id);
    Task<List<Machine>> GetMachinesByStatusAsync(MachineStatus status);
    Task<bool> UpdateMachineStatusAsync(int machineId, MachineStatus status);
    Task<bool> UpdateHeartbeatAsync(int machineId);
}

public interface ILogService
{
    Task<List<LogEntry>> GetLogsAsync(int? machineId = null, Models.LogLevel? level = null, DateTime? from = null, DateTime? to = null, int page = 1, int pageSize = 100);
    Task<LogEntry> CreateLogEntryAsync(LogEntry logEntry);
    Task<List<LogEntry>> GetMachineLogsAsync(int machineId, int count = 100);
    Task<int> GetLogCountByLevelAsync(Models.LogLevel level, DateTime? from = null);
}

public interface ICommandService
{
    Task<List<Command>> GetAllCommandsAsync();
    Task<Command?> GetCommandByIdAsync(int id);
    Task<Command> CreateCommandAsync(Command command);
    Task<bool> UpdateCommandStatusAsync(int commandId, CommandStatus status, string? result = null, string? errorMessage = null);
    Task<List<Command>> GetPendingCommandsAsync();
    Task<List<Command>> GetMachineCommandsAsync(int machineId);
    Task<bool> CancelCommandAsync(int commandId);
}

public interface IRealtimeService
{
    Task BroadcastMachineStatusAsync(int machineId, MachineStatus status);
    Task BroadcastNewLogAsync(LogEntry logEntry);
    Task BroadcastCommandStatusAsync(int commandId, CommandStatus status);
    Task BroadcastHeartbeatAsync(int machineId, DateTime timestamp);
}

public interface INotificationService
{
    Task SendNotificationAsync(string message, string type = "info");
    Task SendMachineAlertAsync(int machineId, string message);
    Task SendSystemAlertAsync(string message);
}