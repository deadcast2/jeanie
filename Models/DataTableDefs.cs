using System.Collections.Generic;

namespace jeanie.Models
{
    public class DataTable
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public DataTableSearch search { get; set; }
        public List<DataTableOrder> order { get; set; }
        public List<DataTableColumn> columns { get; set; }
    }

    public class DataTableSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public class DataTableOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
        public bool IsAsc => dir == "asc";
    }

    public class DataTableColumn
    {
        public string name { get; set; }
    }
}