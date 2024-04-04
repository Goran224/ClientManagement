using ClientAppCore.Enums;
using ClientAppCore.Interfaces;
using ClientAppCore.Models;
using ClientManagementWebApp.Models;
using ClientManagementWebApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace ClientManagementWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IXmlParsingService _xmlParsingService;
        private readonly IClientService _clientService;


        public HomeController(IXmlParsingService xmlParsingService, IClientService clientService)
        {
            _xmlParsingService = xmlParsingService;
            _clientService = clientService;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _clientService.GetAllAsync(pageNumber, pageSize, cancellationToken);

                var viewModel = new ClientListViewModel
                {
                    Clients = result.Items,
                    TotalPages = result.TotalPages,
                    PageNumber = pageNumber
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while exporting a client list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while updating a client.");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ImportXml(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {
                var isClientCreated = await _xmlParsingService.ParseXml(xmlFile);

                if (isClientCreated && ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("File", "Please upload a file.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload a file.");
                return View();
            }
        }

        [HttpPost]
        public IActionResult Export(string model)
        {
            try
            {
                var clientList = JsonConvert.DeserializeObject<List<ClientDto>>(HttpUtility.HtmlDecode(model));
                
                var jsonString = JsonConvert.SerializeObject(clientList);
            
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                return File(bytes, "application/json", "clients.json");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while exporting a client list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while updating a client.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditView(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientDto client)
        {
            ValidateAddresses(client);
            
                try
                {
                    await _clientService.UpdateAsync(client);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred while editing a client.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while editing a client.");
                }

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientDto client)
        {
            ValidateAddresses(client);

                try
                {
                    await _clientService.CreateNewClientAsync(client);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred while creating a client.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while creating a client.");
                }
        }



        private void ValidateAddresses(ClientDto client)
        {
            var homeAddress = client.Addresses.FirstOrDefault(a => a.Type == AddressType.HomeAddress);
            var vacayAddress = client.Addresses.FirstOrDefault(a => a.Type == AddressType.VacayAddress);

            if (vacayAddress != null && vacayAddress.Street == null)
            {
                client.Addresses.Remove(vacayAddress);
            }

            if (homeAddress == null || string.IsNullOrWhiteSpace(homeAddress.Street))
            {
                ModelState.AddModelError("Addresses", "Home address is required.");
            }
        }

    }
}
