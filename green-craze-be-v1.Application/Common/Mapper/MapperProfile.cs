﻿using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Auth;
using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Model.Delivery;
using green_craze_be_v1.Application.Model.OrderCancellationReason;
using green_craze_be_v1.Application.Model.PaymentMethod;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Model.User;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Application.Model.ProductCategory;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Model.Variant;

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

            CreateMap<Delivery, DeliveryDto>();
            CreateMap<CreateDeliveryRequest, Delivery>();
            CreateMap<UpdateDeliveryRequest, Delivery>();

            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<CreatePaymentMethodRequest, PaymentMethod>();
            CreateMap<UpdatePaymentMethodRequest, PaymentMethod>();

            CreateMap<OrderCancellationReason, OrderCancellationReasonDto>();
            CreateMap<CreateOrderCancellationReasonRequest, OrderCancellationReason>();
            CreateMap<UpdateOrderCancellationReasonRequest, OrderCancellationReason>();

            CreateMap<CartItem, CartItemDto>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Product, ProductDto>();
            // Unit
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUnitRequest, Unit>();
            CreateMap<UpdateUnitRequest, Unit>();
            // Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<UpdateBrandRequest, Brand>();
            // ProductCategory
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<CreateProductCategoryRequest, ProductCategory>();
            CreateMap<UpdateProductCategoryRequest, ProductCategory>();
            // Product
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            // Product Image
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<ProductImageDto, ProductImage>();
            // Product
            CreateMap<Variant, VariantDto>();
            CreateMap<CreateVariantRequest, Variant>();
            CreateMap<UpdateVariantRequest, Variant>();
            // Sale
            CreateMap<Sale, SaleDto>();
            CreateMap<CreateSaleRequest, Sale>();
            CreateMap<UpdateSaleRequest, Sale>();
        }
    }
}