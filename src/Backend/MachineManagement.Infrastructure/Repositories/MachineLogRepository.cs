using Microsoft.EntityFrameworkCore;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;
using MachineManagement.Infrastructure.Data;

namespace MachineManagement.Infrastructure.Repositories;

public class MachineLogRepository : IMachineLogRepository
{
    private readonly MachineManagementDbContext _context;

    public MachineLogRepository(MachineManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MachineLog>> GetLogsByMachineAsync(int machineId)
    {
        return await _context.MachineLogs
            .Where(l => l.MachineId == machineId)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<MachineLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.MachineLogs
            .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<MachineLog>> GetLogsByLevelAsync(string logLevel)
    {
        return await _context.MachineLogs
            .Where(l => l.LogLevel == logLevel)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }

    public async Task<MachineLog> AddLogAsync(MachineLog log)
    {
        await _context.MachineLogs.AddAsync(log);
        return log;
    }

    public async Task<IEnumerable<MachineLog>> GetRecentLogsAsync(int count = 100)
    {
        return await _context.MachineLogs
            .OrderByDescending(l => l.Timestamp)
            .Take(count)
            .ToListAsync();
    }
}