using MachineManagement.Core.Entities;

namespace MachineManagement.Core.Services;

public interface ILogService
{
    Task<MachineLog> AddLogAsync(MachineLog log);
    Task<IEnumerable<MachineLog>> GetLogsByMachineAsync(int machineId);
    Task<IEnumerable<MachineLog>> GetRecentLogsAsync(int count = 100);
    Task<IEnumerable<MachineLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
}