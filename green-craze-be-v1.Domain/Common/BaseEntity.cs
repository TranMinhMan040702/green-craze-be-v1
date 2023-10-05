using green_craze_be_v1.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Common
{
    public class BaseEntity<T> : IEntity<T>
    {
        [Column("id")]
        public T Id { get; set; }
    }
}