using System;
using System.Collections.Generic;

namespace ECommerce.BLL.Request
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public List<DataTableColumn> Columns { get; set; }
        public List<DataTableOrder> Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public int PageIndex
        {
            get { return Start / Length; }
        }

        public DataTableSearch Search { get; set; }
        public Guid? RoleId { get; set; } = Guid.Empty;
        public int BusinessSiteServiceId { get; set; }
        public int FacilityOptionId { get; set; }
        public int Type { get; set; }
    }

    public class DataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableSearch Search { get; set; }
    }

    public class DataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; } // "asc" or "desc"
    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
}
