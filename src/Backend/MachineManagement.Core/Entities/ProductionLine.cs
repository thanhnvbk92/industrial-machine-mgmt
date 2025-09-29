namespace MachineManagement.Core.Entities;

public class ProductionLine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public int BuyerId { get; set; }

    // Navigation properties
    public Buyer Buyer { get; set; } = null!;
    public ICollection<Station> Stations { get; set; } = new List<Station>();
}