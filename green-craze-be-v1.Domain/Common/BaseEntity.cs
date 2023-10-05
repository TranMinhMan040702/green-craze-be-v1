using green_craze_be_v1.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Common
{
    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
    }
}