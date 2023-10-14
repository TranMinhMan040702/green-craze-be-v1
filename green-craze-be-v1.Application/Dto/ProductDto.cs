﻿using green_craze_be_v1.Application.Common.Enums;

namespace green_craze_be_v1.Application.Dto
{
    public class ProductDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public long? SaleId { get; set; }
        public long BrandId { get; set; }
        public long UnitId { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public double Rating { get; set; }
        public List<ProductImageDto> Images { get; set; }
        public string Slug { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }
}
