﻿using green_craze_be_v1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class AddressDto
    {
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public bool IsDefault { get; set; }
        public bool Status { get; set; }
        public ProvinceDto Province { get; set; }
        public DistrictDto District { get; set; }
        public WardDto Ward { get; set; }
    }
}