using MachineManagement.Core.Entities;

namespace MachineManagement.Core.Interfaces;

public interface IMachineLogRepository
{
    Task<IEnumerable<MachineLog>> GetLogsByMachineAsync(int machineId);
    Task<IEnumerable<MachineLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<MachineLog>> GetLogsByLevelAsync(string logLevel);
    Task<MachineLog> AddLogAsync(MachineLog log);
    Task<IEnumerable<MachineLog>> GetRecentLogsAsync(int count = 100);
}