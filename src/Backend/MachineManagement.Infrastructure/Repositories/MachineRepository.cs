using Microsoft.EntityFrameworkCore;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;
using MachineManagement.Infrastructure.Data;

namespace MachineManagement.Infrastructure.Repositories;

public class MachineRepository : GenericRepository<Machine>, IMachineRepository
{
    public MachineRepository(MachineManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Machine>> GetMachinesByStationAsync(int stationId)
    {
        return await _dbSet
            .Where(m => m.StationId == stationId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Machine>> GetMachinesByStatusAsync(MachineStatus status)
    {
        return await _dbSet
            .Where(m => m.Status == status)
            .ToListAsync();
    }

    public async Task<Machine?> GetBySerialNumberAsync(string serialNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.SerialNumber == serialNumber);
    }

    public async Task<IEnumerable<Machine>> GetMachinesWithLogsAsync()
    {
        return await _dbSet
            .Include(m => m.MachineLogs.OrderByDescending(l => l.Timestamp).Take(10))
            .ToListAsync();
    }
}