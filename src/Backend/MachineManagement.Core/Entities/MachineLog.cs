namespace MachineManagement.Core.Entities;

public class MachineLog
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string LogLevel { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? Source { get; set; }
    public string? Category { get; set; }
    public MachineStatus? MachineStatusAtTime { get; set; }

    // Foreign key
    public int MachineId { get; set; }

    // Navigation properties
    public Machine Machine { get; set; } = null!;
}