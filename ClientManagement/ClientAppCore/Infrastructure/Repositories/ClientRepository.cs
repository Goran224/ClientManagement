using ClientAppCore.Entities;
using ClientAppCore.Infrastructure.Data;
using ClientAppCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClientAppCore.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Clients.Include(x => x.Addresses).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Clients.Include(x => x.Addresses).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Client entity, CancellationToken cancellationToken)
        {
            await _context.Clients.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Client entity, CancellationToken cancellationToken)
        {
            _context.Clients.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Client entity, CancellationToken cancellationToken)
        {
            _context.Clients.Remove(entity);
            await Task.CompletedTask;
        }
    }

}
