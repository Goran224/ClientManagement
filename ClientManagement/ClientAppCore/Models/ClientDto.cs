namespace ClientAppCore.Models
{
    public class ClientDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public DateTime BirthDate { get; set; }
    }
}