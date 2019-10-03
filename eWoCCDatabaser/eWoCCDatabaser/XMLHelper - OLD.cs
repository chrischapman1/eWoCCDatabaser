//This code was made for the group project by myself and Lachlan Green
using System.Xml;
using System.Xml.Serialization;
using Schemas;
using System;
using eWoCCDatabaser;

public class XMLHelper{
    public data data_object;

    public XMLHelper(String input){
        try
        {
            XmlSerializer ser = new XmlSerializer(typeof(data));
            using (XmlReader reader = XmlReader.Create(input))
            {
                data_object = (data)ser.Deserialize(reader);
            }
        }
        catch (Exception e)
        {
            ErrorHandling.logError("_modelInputs.xml could not be found", e);
        }
    } 

    public data getDataObject()
    {
        return data_object;
    }

    public dataRunParameters[] getRunParameters()
    {
        return data_object.runParameters;
    }

} 
