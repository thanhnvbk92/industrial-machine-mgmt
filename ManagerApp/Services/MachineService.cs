using Microsoft.EntityFrameworkCore;
using ManagerApp.Data;
using ManagerApp.Models;

namespace ManagerApp.Services;

public class MachineService : IMachineService
{
    private readonly ApplicationDbContext _context;

    public MachineService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Machine>> GetAllMachinesAsync()
    {
        return await _context.Machines
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Machine?> GetMachineByIdAsync(int id)
    {
        return await _context.Machines
            .Include(m => m.LogEntries.OrderByDescending(l => l.Timestamp).Take(10))
            .Include(m => m.Commands.OrderByDescending(c => c.CreatedAt).Take(10))
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Machine> CreateMachineAsync(Machine machine)
    {
        machine.CreatedAt = DateTime.UtcNow;
        machine.UpdatedAt = DateTime.UtcNow;
        
        _context.Machines.Add(machine);
        await _context.SaveChangesAsync();
        return machine;
    }

    public async Task<Machine> UpdateMachineAsync(Machine machine)
    {
        machine.UpdatedAt = DateTime.UtcNow;
        
        _context.Machines.Update(machine);
        await _context.SaveChangesAsync();
        return machine;
    }

    public async Task<bool> DeleteMachineAsync(int id)
    {
        var machine = await _context.Machines.FindAsync(id);
        if (machine == null)
            return false;

        _context.Machines.Remove(machine);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Machine>> GetMachinesByStatusAsync(MachineStatus status)
    {
        return await _context.Machines
            .Where(m => m.Status == status)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<bool> UpdateMachineStatusAsync(int machineId, MachineStatus status)
    {
        var machine = await _context.Machines.FindAsync(machineId);
        if (machine == null)
            return false;

        machine.Status = status;
        machine.UpdatedAt = DateTime.UtcNow;
        
        if (status == MachineStatus.Online)
        {
            machine.IsConnected = true;
            machine.LastHeartbeat = DateTime.UtcNow;
        }
        else if (status == MachineStatus.Offline)
        {
            machine.IsConnected = false;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateHeartbeatAsync(int machineId)
    {
        var machine = await _context.Machines.FindAsync(machineId);
        if (machine == null)
            return false;

        machine.LastHeartbeat = DateTime.UtcNow;
        machine.IsConnected = true;
        machine.UpdatedAt = DateTime.UtcNow;
        
        if (machine.Status == MachineStatus.Offline)
        {
            machine.Status = MachineStatus.Online;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}