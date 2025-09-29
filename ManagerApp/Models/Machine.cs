using System.ComponentModel.DataAnnotations;

namespace ManagerApp.Models;

public class Machine
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Location { get; set; } = string.Empty;
    
    public MachineStatus Status { get; set; } = MachineStatus.Offline;
    
    public DateTime LastHeartbeat { get; set; } = DateTime.UtcNow;
    
    [StringLength(20)]
    public string Version { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string IpAddress { get; set; } = string.Empty;
    
    public bool IsConnected { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<LogEntry> LogEntries { get; set; } = new List<LogEntry>();
    public virtual ICollection<Command> Commands { get; set; } = new List<Command>();
}

public class LogEntry
{
    public int Id { get; set; }
    
    public int MachineId { get; set; }
    
    public LogLevel Level { get; set; }
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [StringLength(100)]
    public string Source { get; set; } = string.Empty;
    
    public string? AdditionalData { get; set; }
    
    // Navigation properties
    public virtual Machine Machine { get; set; } = null!;
}

public class Command
{
    public int Id { get; set; }
    
    public int MachineId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string CommandType { get; set; } = string.Empty;
    
    public string? Parameters { get; set; }
    
    public CommandStatus Status { get; set; } = CommandStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ExecutedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public string? Result { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    [StringLength(100)]
    public string CreatedBy { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Machine Machine { get; set; } = null!;
}