using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;
using MachineManagement.Core.Services;

namespace MachineManagement.Infrastructure.Services;

public class MachineService : IMachineService
{
    private readonly IUnitOfWork _unitOfWork;

    public MachineService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _unitOfWork.Machines.GetAllAsync();
    }

    public async Task<Machine?> GetMachineByIdAsync(int id)
    {
        return await _unitOfWork.Machines.GetByIdAsync(id);
    }

    public async Task<Machine> CreateMachineAsync(Machine machine)
    {
        var result = await _unitOfWork.Machines.AddAsync(machine);
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task<Machine> UpdateMachineAsync(Machine machine)
    {
        machine.UpdatedAt = DateTime.UtcNow;
        var result = await _unitOfWork.Machines.UpdateAsync(machine);
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task DeleteMachineAsync(int id)
    {
        await _unitOfWork.Machines.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Machine>> GetMachinesByStationAsync(int stationId)
    {
        return await _unitOfWork.Machines.GetMachinesByStationAsync(stationId);
    }

    public async Task<Machine> UpdateMachineStatusAsync(int machineId, MachineStatus status)
    {
        var machine = await _unitOfWork.Machines.GetByIdAsync(machineId);
        if (machine == null)
        {
            throw new ArgumentException($"Machine with ID {machineId} not found.");
        }

        machine.Status = status;
        machine.UpdatedAt = DateTime.UtcNow;

        var result = await _unitOfWork.Machines.UpdateAsync(machine);
        await _unitOfWork.SaveChangesAsync();
        
        return result;
    }
}