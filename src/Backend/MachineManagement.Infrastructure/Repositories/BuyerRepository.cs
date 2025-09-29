using Microsoft.EntityFrameworkCore;
using MachineManagement.Core.Entities;
using MachineManagement.Core.Interfaces;
using MachineManagement.Infrastructure.Data;

namespace MachineManagement.Infrastructure.Repositories;

public class BuyerRepository : GenericRepository<Buyer>, IBuyerRepository
{
    public BuyerRepository(MachineManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Buyer>> GetBuyersWithLinesAsync()
    {
        return await _dbSet
            .Include(b => b.ProductionLines)
            .ToListAsync();
    }

    public async Task<Buyer?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b => b.Code == code);
    }
}