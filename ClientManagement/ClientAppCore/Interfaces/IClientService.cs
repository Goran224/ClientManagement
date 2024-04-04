using ClientAppCore.Entities;
using ClientAppCore.Models; 

namespace ClientAppCore.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto> CreateNewClientAsync(ClientDto client, CancellationToken cancellationToken = default);
        Task<bool> InsertClientsAsync(List<ClientDto> clientDtoList, CancellationToken cancellationToken = default);
        Task<PaginatedResult<ClientDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<ClientDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(ClientDto entity, CancellationToken cancellationToken = default);
    }
}
