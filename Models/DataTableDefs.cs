using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jeanie.Models
{
    public class DataTable
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string search { get; set; }
        public List<DataTableOrder> order { get; set; }
        public List<DataTableColumn> columns { get; set; }
    }

    public class DataTableOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class DataTableColumn
    {
        public string name { get; set; }
    }
}