namespace Application.Abstractions;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    Task RollbackAsync(CancellationToken cancellationToken = default);
}