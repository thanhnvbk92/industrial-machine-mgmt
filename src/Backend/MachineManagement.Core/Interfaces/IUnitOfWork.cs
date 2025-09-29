namespace MachineManagement.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBuyerRepository Buyers { get; }
    IMachineRepository Machines { get; }
    IMachineLogRepository MachineLogs { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}