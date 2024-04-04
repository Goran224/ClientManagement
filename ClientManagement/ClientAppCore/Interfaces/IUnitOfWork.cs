namespace ClientAppCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync();
    }

}
