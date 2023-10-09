using green_craze_be_v1.Application.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Current => DateTime.UtcNow;
    }
}