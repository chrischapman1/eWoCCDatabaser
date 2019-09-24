using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    
    class XMLtoDataTable
    {
        private DataTable dataTable;

        public DataTable getDataTable()
        {
            return dataTable;
        }

        public XMLtoDataTable(data xml)
        {
            Console.Write(xml);
        }

    }
}
