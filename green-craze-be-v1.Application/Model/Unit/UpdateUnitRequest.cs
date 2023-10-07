using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Unit
{
    public class UpdateUnitRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }
    }
}