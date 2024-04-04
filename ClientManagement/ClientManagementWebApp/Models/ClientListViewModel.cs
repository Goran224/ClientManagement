
namespace ClientManagementWebApp.Models
{
    public class ClientListViewModel
    {
        public List<ClientAppCore.Models.ClientDto> Clients { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
    }
}
