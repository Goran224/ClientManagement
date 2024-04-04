using ClientAppCore.Enums;

namespace ClientAppCore.Models
{
    public class AddressDto
    {
        public string Street { get; set; }
        public AddressType Type { get; set; }
    }
}
