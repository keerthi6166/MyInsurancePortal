using AutoMapper;
using MyInsurancePortal.DtoModels;
using MyInsurancePortal.Models;

namespace MyInsurancePortal.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Claim mappings
            CreateMap<Claim, ClaimDto>().ReverseMap(); // reverse allows mapping in both ways.

            // Policy mappings
            CreateMap<Policy, PolicyDto>().ReverseMap();

            // Customer mappings
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // Payment mappings
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
    

