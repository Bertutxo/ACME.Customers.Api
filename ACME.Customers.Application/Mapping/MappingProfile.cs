using ACME.Customers.Application.DTOs;
using ACME.Customers.Core.Entities;
using AutoMapper;

namespace ACME.Customers.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Comerciales
            CreateMap<SalesRep, SalesRepDto>().ReverseMap();
            CreateMap<SalesRepCreateDto, SalesRep>();
            CreateMap<SalesRepUpdateDto, SalesRep>();

            // Clientes
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<ClientCreateDto, Client>();
            CreateMap<ClientUpdateDto, Client>();
        }
    }
}