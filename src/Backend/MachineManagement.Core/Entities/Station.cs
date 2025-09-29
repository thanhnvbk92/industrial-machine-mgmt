namespace MachineManagement.Core.Entities;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public int ProductionLineId { get; set; }

    // Navigation properties
    public ProductionLine ProductionLine { get; set; } = null!;
    public ICollection<Machine> Machines { get; set; } = new List<Machine>();
}