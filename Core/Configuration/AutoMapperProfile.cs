using AutoMapper;
using InventoryManager.API.Data.Dto.Product;
using InventoryManager.API.Data.Models;

namespace InventoryManager.API.Core.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ResponseDto>();
            CreateMap<RequestDto, Product>();
        }
    }
}
