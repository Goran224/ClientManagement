using ClientAppCore.Enums;
using ClientShared;

namespace ClientAppCore.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; private set; }
        public ICollection<Address> Addresses { get; private set; } 
        public DateTime BirthDate { get; private set; }

        protected Client() { }

        private Client(string name, DateTime birthDate, List<Address> addresses)
        {
            ValidateConstructorArguments(name, addresses, birthDate);

            Name = name;
            BirthDate = birthDate;
            Addresses = addresses;
        }

        public static Client CreateNewClient(string name, DateTime birthDate, List<Address> addresses)
        {
            return new Client(name, birthDate, addresses);
        }

        public void UpdateName(string newName)
        {
            ValidateName(newName);
            Name = newName;
        }

        public void UpdateBirthDate(DateTime birthDate)
        {
            ValidateBirthDate(birthDate);
            BirthDate = birthDate;
        }

        public void AddAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            Addresses.Add(address);
        }

        public void RemoveAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            Addresses.Remove(address);
        }
        public void ClearAddresses()
        {
            Addresses.Clear();
        }   
        private void ValidateConstructorArguments(string name, List<Address> addresses, DateTime birthDate)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(ErrorMessages.NameNullOrEmpty, nameof(name));
            }

            if (addresses == null || !addresses.Any())
            {
                throw new ArgumentException(ErrorMessages.AddressNotSpecified, nameof(addresses));
            }

            if (!addresses.Any(a => a.Type == AddressType.HomeAddress))
            {
                throw new ArgumentException(ErrorMessages.HomeAddressNull, nameof(addresses));
            }

            ValidateBirthDate(birthDate);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(ErrorMessages.NameNullOrEmpty, nameof(name));
            }
        }

        public void ValidateBirthDate(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
            {
                throw new ArgumentNullException(nameof(birthDate), "Birth date cannot be null.");
            }

            if (birthDate > DateTime.Now)
            {
                throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
            }
        }
    }
}

