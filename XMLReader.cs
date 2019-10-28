using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace eWoCCDatabaser
{
    class XMLReader
    {
        private data data_object;

        public XMLReader()
        { }

        public XMLReader(String fileLocation)
        {
            XmlSerializer ser = new XmlSerializer(typeof(data));
            using (XmlReader reader = XmlReader.Create(fileLocation))
            {
                data_object = (data)ser.Deserialize(reader);
            }

        }

        public data getDataObject()
        {
            return data_object;
        }
    }
}
