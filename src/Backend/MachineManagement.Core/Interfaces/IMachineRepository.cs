using MachineManagement.Core.Entities;

namespace MachineManagement.Core.Interfaces;

public interface IMachineRepository : IGenericRepository<Machine>
{
    Task<IEnumerable<Machine>> GetMachinesByStationAsync(int stationId);
    Task<IEnumerable<Machine>> GetMachinesByStatusAsync(MachineStatus status);
    Task<Machine?> GetBySerialNumberAsync(string serialNumber);
    Task<IEnumerable<Machine>> GetMachinesWithLogsAsync();
}