using ClientAppCore.Infrastructure.Data;
using ClientAppCore.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace ClientAppCore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(AppDbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_transaction is not null)
                {
                    int affectedRows = await _context.SaveChangesAsync(cancellationToken);
                    await _transaction.CommitAsync(cancellationToken); 
                    return affectedRows;
                }
                else
                {
                 
                    return await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await RollbackAsync(); 
                throw; 
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction is not null)
                {
                    await _transaction.RollbackAsync(); 
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw; 
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null; 
            }
        }

        public void BeginTransaction()
        {
            if (_transaction is null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose(); 
            _context.Dispose(); 
        }
    }

}

