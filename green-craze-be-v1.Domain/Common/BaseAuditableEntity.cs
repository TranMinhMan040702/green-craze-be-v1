using green_craze_be_v1.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Common
{
    public class BaseAuditableEntity<T> : BaseEntity<T>, IAuditableEntity<T>
    {
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }
    }
}