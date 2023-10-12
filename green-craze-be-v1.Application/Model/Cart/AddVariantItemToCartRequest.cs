using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Cart
{
    public class AddVariantItemToCartRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public long VariantId { get; set; }
        public int Quantity { get; set; }
    }
}