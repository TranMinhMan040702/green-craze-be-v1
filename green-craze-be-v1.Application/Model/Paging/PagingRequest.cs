using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Paging
{
    public class PagingRequest
    {
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }

        public bool IsSortAccending { get; set; } = true;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 1000;
        public string ColumnName { get; set; } = "Id";
    }
}