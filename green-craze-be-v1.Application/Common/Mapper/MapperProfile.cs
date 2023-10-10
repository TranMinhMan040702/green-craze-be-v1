using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Common.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDto>();
            // Unit
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUnitRequest, Unit>();
            CreateMap<UpdateUnitRequest, Unit>();
            // Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<UpdateBrandRequest, Brand>();
        }
    }
}