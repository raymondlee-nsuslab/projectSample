using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiReferences
{
    public class DataTableRequest
    {
        public DatatableSearch Search { get; set; }
        public string Name { get; set; }
    }
    public class DatatableSearch
    {
        public string Value { get; set; }
    }

    public class DatatableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class Students
    {
        public string Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<DataTableRequest> Columns { get; set; }
        public List<DatatableOrder> Order { get; set; }
    }
}
