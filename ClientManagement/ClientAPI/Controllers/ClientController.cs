using ClientAppCore.Interfaces;
using ClientAppCore.Models;
using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ClientDto>> CreateClient([FromBody] ClientDto client)
        {
            try
            {
                var createdClient = await _clientService.CreateNewClientAsync(client);
                return Ok(createdClient);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating a client.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while creating a client.");
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ClientDto>> GetById(int id)
        {
            try
            {
                var client = await _clientService.GetByIdAsync(id);
                if (client == null)
                    return NotFound();

                return Ok(client);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving a client by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while retrieving a client by ID.");
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients(int pagenumber, int pageSize)
        {
            try
            {
                var clients = await _clientService.GetAllAsync(pagenumber, pageSize);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all clients.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while retrieving all clients.");
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<bool>> UpdateClient([FromBody] ClientDto client)
        {
            try
            {
                
                var isUpdated = await _clientService.UpdateAsync(client);
                if (!isUpdated)
                    return BadRequest(); 

                return Ok(isUpdated);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating a client.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while updating a client.");
            }
        }     
    }
}
