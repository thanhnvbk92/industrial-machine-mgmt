namespace MachineManagement.Core.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MachineStatus Status { get; set; } = MachineStatus.Stopped;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }

    // Foreign key
    public int StationId { get; set; }

    // Navigation properties
    public Station Station { get; set; } = null!;
    public ICollection<MachineLog> MachineLogs { get; set; } = new List<MachineLog>();
}

public enum MachineStatus
{
    Stopped = 0,
    Running = 1,
    Idle = 2,
    Maintenance = 3,
    Error = 4,
    Warning = 5
}