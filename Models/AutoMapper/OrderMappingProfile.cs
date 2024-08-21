using AutoMapper;
using CodingChallangeWebApi.Models.DataEntityModels;
using CodingChallangeWebApi.Models.DTOModels;
namespace CodingChallangeWebApi.Models.OrderMappingProfile
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderDTO, Order>();
        }
    }
}
