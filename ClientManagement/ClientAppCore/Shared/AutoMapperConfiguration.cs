using AutoMapper;
using ClientAppCore.Entities;
using ClientAppCore.Models;
using System.Linq;

namespace ClientAppCore.Shared
{
    internal class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientDto, Client>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map Id property
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                    .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses.Select(a => new Address(a.Street))));

                cfg.CreateMap<Client, ClientDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map Id property
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                    .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses.Select(a => new AddressDto { Street = a.Street, Type = a.Type })));

                cfg.CreateMap<AddressDto, Address>()
                    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

                cfg.CreateMap<Address, AddressDto>()
                    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
            });

            return config.CreateMapper();
        }
    }
}