using MachineManagement.Core.Entities;

namespace MachineManagement.Core.Services;

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<Machine?> GetMachineByIdAsync(int id);
    Task<Machine> CreateMachineAsync(Machine machine);
    Task<Machine> UpdateMachineAsync(Machine machine);
    Task DeleteMachineAsync(int id);
    Task<IEnumerable<Machine>> GetMachinesByStationAsync(int stationId);
    Task<Machine> UpdateMachineStatusAsync(int machineId, MachineStatus status);
}