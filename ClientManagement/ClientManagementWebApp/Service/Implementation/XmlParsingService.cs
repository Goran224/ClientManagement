using ClientAppCore.Enums;
using ClientAppCore.Interfaces;
using ClientAppCore.Models;
using ClientManagementWebApp.Service.Interfaces;
using Serilog;
using System.Net.Security;
using System.Xml.Linq;

namespace ClientManagementWebApp.Service.Implementation
{
    public class XmlParsingService : IXmlParsingService
    {
        private readonly IClientService _clientService;

        public XmlParsingService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<bool> ParseXml(IFormFile xmlStream)
        {
            if (xmlStream == null || xmlStream.Length == 0)
                return false; 

            try
            {

                var clientDtoList = ProcessTheXMl(xmlStream);

                return await _clientService.InsertClientsAsync(clientDtoList);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        private List<ClientDto> ProcessTheXMl(IFormFile xmlStream)
        {
            var clientDtoList = new List<ClientDto>();

            using (var reader = new StreamReader(xmlStream.OpenReadStream()))
            {
                XDocument doc = XDocument.Load(reader);

                foreach (var clientElement in doc.Root.Elements("client"))
                {
                    string name = clientElement.Element("name").Value;
                    DateTime birthDate = DateTime.Now; 
                    List<AddressDto> addresses = new List<AddressDto>();


                    foreach (var addressElement in clientElement.Element("addresses").Elements("address"))
                    {
                        int typeValue = int.Parse(addressElement.Attribute("type").Value);

                        AddressType type = Enum.TryParse<AddressType>(typeValue.ToString(), out var result) ? result : AddressType.HomeAddress;

                        type = Enum.IsDefined(typeof(AddressType), typeValue) ? (AddressType)typeValue : AddressType.HomeAddress;

                        AddressDto addressDto = new AddressDto
                        {
                            Street = addressElement.Value,
                            Type = type
                        };
                        addresses.Add(addressDto);
                    }

                    ClientDto clientDto = new ClientDto
                    {
                        Name = name,
                        Addresses = addresses,
                        BirthDate = birthDate
                    };

                    clientDtoList.Add(clientDto);
                }

                return clientDtoList;

            }

        }
    }
}