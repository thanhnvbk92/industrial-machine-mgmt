using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;
using MachineManagement.Core.Services;

namespace MachineManagement.Infrastructure.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MachineLog> AddLogAsync(MachineLog log)
    {
        var result = await _unitOfWork.MachineLogs.AddLogAsync(log);
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<MachineLog>> GetLogsByMachineAsync(int machineId)
    {
        return await _unitOfWork.MachineLogs.GetLogsByMachineAsync(machineId);
    }

    public async Task<IEnumerable<MachineLog>> GetRecentLogsAsync(int count = 100)
    {
        return await _unitOfWork.MachineLogs.GetRecentLogsAsync(count);
    }

    public async Task<IEnumerable<MachineLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.MachineLogs.GetLogsByDateRangeAsync(startDate, endDate);
    }
}