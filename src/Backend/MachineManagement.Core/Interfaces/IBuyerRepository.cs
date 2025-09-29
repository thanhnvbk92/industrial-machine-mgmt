using MachineManagement.Core.Entities;

namespace MachineManagement.Core.Interfaces;

public interface IBuyerRepository : IGenericRepository<Buyer>
{
    Task<IEnumerable<Buyer>> GetBuyersWithLinesAsync();
    Task<Buyer?> GetByCodeAsync(string code);
}