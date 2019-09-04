using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schemas;

namespace eWoCCDatabaser
{
    interface IXMLDataBoolean
    {
        void dataTableBoolean();
    }

    public class datacoalChainParameters : IXMLDataBoolean
    {
        public void dataTableBoolean(dataLogSwitchboard[] dataLogSwitchboard)
        {

        }

        void IXMLDataBoolean.dataTableBoolean()
        {
            throw new NotImplementedException();
        }
    }
}
