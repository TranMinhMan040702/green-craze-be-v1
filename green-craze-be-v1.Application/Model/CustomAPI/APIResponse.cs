using green_craze_be_v1.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.CustomAPI
{
    public class APIResponse<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }

        public APIResponse(T data, int status)
        {
            Data = data;
            Status = status;
        }

        public static APIResponse<T> Create(T data, int status)
        {
            return new APIResponse<T>(data, status);
        }
    }
}