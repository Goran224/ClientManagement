using ClientAppCore.Enums;
using ClientShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAppCore.Entities
{
    public class Address : BaseEntity
    {
        public Address(string street)
        {
            Street = street;

        }

        public string Street { get; set; }
        public AddressType Type { get; set; } 

        public int ClientId { get; set; }
        public Client Client { get; set; } 

    }
}
