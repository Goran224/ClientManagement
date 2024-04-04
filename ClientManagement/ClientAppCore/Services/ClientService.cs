using AutoMapper;
using ClientAppCore.Entities;
using ClientAppCore.Enums;
using ClientAppCore.Interfaces;
using ClientAppCore.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using System.Threading;

namespace ClientAppCore.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Client> _clientRepository;
        private readonly IMapper _mapper;


        public ClientService(IUnitOfWork unitOfWork, IGenericRepository<Client> clientRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDto> CreateNewClientAsync(ClientDto clientDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var mappedClient = _mapper.Map<Client>(clientDto);
                var addressesList = new List<Address>(mappedClient.Addresses);
                var newClient = Client.CreateNewClient(mappedClient.Name, mappedClient.BirthDate, addressesList);

                await _clientRepository.AddAsync(newClient, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return _mapper.Map<ClientDto>(newClient);
            }
            catch (DbUpdateException ex)
            {
                Log.Error($"Database update error: {ex}");
                await _unitOfWork.RollbackAsync();
                throw new ApplicationException("Error saving to the database. Please try again or contact support.", ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ClientDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingClient = await _clientRepository.GetByIdAsync(entity.Id, cancellationToken);
                var clientToUpdate = _mapper.Map<Client>(entity);
                if (existingClient == null)
                {
                    return false;
                }

                existingClient.UpdateName(clientToUpdate.Name);
                existingClient.UpdateBirthDate(clientToUpdate.BirthDate);
                existingClient.ClearAddresses();

                if (clientToUpdate.Addresses.Any(address => address.Type == AddressType.HomeAddress))
                {
                    foreach (var address in clientToUpdate.Addresses)
                    {
                        existingClient.AddAddress(address);
                    }
                }
                else
                {
                    return false;
                }

                await _clientRepository.UpdateAsync(existingClient, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PaginatedResult<ClientDto>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var clientDtos = new List<ClientDto>();
                var clients = await _clientRepository.GetAllAsync(cancellationToken);
                if (clients != null && clients.Count() > 0)
                {
                    foreach (var client in clients)
                    {
                       ClientDto clientDto =  _mapper.Map<ClientDto>(client);
                       clientDtos.Add(clientDto);   
                    }
                }
                var filteredClients = clientDtos
                                         .OrderByDescending(clientDto => clientDto.Name)
                                         .ThenBy(clientDto => clientDto.BirthDate)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToList();

                var totalClients = clientDtos.Count; // Total number of clients

                var totalPages = (int)Math.Ceiling((double)totalClients / pageSize);

                return new PaginatedResult<ClientDto>(filteredClients, pageNumber, totalPages);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClientDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(id, cancellationToken);
                return _mapper.Map<ClientDto>(client);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertClientsAsync(List<ClientDto> clientDtoList, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var clientDto in clientDtoList)
                {
                    var mappedClient = _mapper.Map<Client>(clientDto);
                    var addressesList = new List<Address>(mappedClient.Addresses);
                    var newClient = Client.CreateNewClient(mappedClient.Name, mappedClient.BirthDate, addressesList);

                    await _clientRepository.AddAsync(newClient, cancellationToken);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateException ex)
            {
                Log.Error($"Database update error: {ex}");
                await _unitOfWork.RollbackAsync();
                throw new ApplicationException("Error saving to the database. Please try again or contact support.", ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

    }
}
