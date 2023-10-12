using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Auth;
using green_craze_be_v1.Application.Model.Delivery;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Model.User;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Common.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AppUser, UserDto>().ForMember(des => des.Phone, act => act.MapFrom(x => x.PhoneNumber));
            CreateMap<Staff, StaffDto>();
            CreateMap<UpdateUserRequest, AppUser>().ForMember(des => des.PhoneNumber, act => act.MapFrom(x => x.Phone));
            CreateMap<CreateStaffRequest, AppUser>().ForMember(des => des.PhoneNumber, act => act.MapFrom(x => x.Phone));
            CreateMap<RegisterRequest, AppUser>();

            CreateMap<Delivery, DeliveryDto>();
            CreateMap<CreateDeliveryRequest, Delivery>();
            CreateMap<UpdateDeliveryRequest, Delivery>();

            CreateMap<Product, ProductDto>();
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUnitRequest, Unit>();
        }
    }
}