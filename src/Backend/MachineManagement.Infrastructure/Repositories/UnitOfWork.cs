using Microsoft.EntityFrameworkCore.Storage;
using MachineManagement.Core.Interfaces;
using MachineManagement.Infrastructure.Data;
using MachineManagement.Infrastructure.Repositories;

namespace MachineManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MachineManagementDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(MachineManagementDbContext context)
    {
        _context = context;
        Buyers = new BuyerRepository(_context);
        Machines = new MachineRepository(_context);
        MachineLogs = new MachineLogRepository(_context);
    }

    public IBuyerRepository Buyers { get; private set; }
    public IMachineRepository Machines { get; private set; }
    public IMachineLogRepository MachineLogs { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}