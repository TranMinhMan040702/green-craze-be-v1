using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Statistic
{
    public class StatisticTopSellingProductYearResponse
    {
        public string Date { get; set; }
        public Dictionary<string, int> Products {  get; set; }

        public StatisticTopSellingProductYearResponse(string date, Dictionary<string, int> products)
        {
            Date = date;
            Products = products;
        }
    }
}
