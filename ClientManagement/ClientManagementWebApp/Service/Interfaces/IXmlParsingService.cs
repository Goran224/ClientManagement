namespace ClientManagementWebApp.Service.Interfaces
{
    public interface IXmlParsingService
    {
        Task<bool> ParseXml(IFormFile xmlStream); 
    }
}
