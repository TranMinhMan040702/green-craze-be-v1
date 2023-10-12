﻿using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.Auth;
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

            CreateMap<Address, AddressDto>();
            CreateMap<Ward, WardDto>();
            CreateMap<District, DistrictDto>();
            CreateMap<Province, ProvinceDto>();
            CreateMap<CreateAddressRequest, Address>();
            CreateMap<UpdateAddressRequest, Address>();

            CreateMap<Product, ProductDto>();
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUnitRequest, Unit>();
        }
    }
}