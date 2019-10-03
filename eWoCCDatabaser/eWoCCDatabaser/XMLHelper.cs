using System;
using eWoCCDatabaser;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;

public class XMLHelper{
    
    public XMLHelper()
    {

    }

    //This method is heavily based off the code found here: https://dotnetfiddle.net/wfo4Fz
    public DataTable XMLtoDataTable(String input, String dataType, String scenarioName){
        try
        {
            XDocument  doc = XDocument.Parse(System.IO.File.ReadAllText(input));
            var result = doc.Descendants(dataType);
            List<XElement> data = result.ToList();
            //Checks if there is more than one element. If so, recurses
            if (data.Count > 1)
            {
                return (DataTable)XMLtoDataTableHelper(data, dataType + "_" + scenarioName);
            }
            else
            {
                return (DataTable)XMLtoDataTableOneChild(data, data.ElementAt(0).Name.LocalName, scenarioName);
            }
        }
        catch (Exception e)
        {
            ErrorHandling.logError("_modelInputs.xml could not be found", e);
        }
        return null;
    }

    private object ConvertToDataTable(List<XElement> data)
    {
        throw new NotImplementedException();
    }

    private DataTable XMLtoDataTableOneChild(IEnumerable<XElement> data, String name, String scenarioName)
    {
        DataTable table = new DataTable();
        table.TableName = name;

        foreach (XAttribute attribute in data.First().Attributes())
        {
            table.Columns.Add(attribute.Name.ToString());
        }

        for (int i=0; i < data.Count(); i++)
        {
            
        }
        return table;
    }

    //This method is heavily based off the code found here: https://dotnetfiddle.net/wfo4Fz
    private DataTable XMLtoDataTableHelper(IEnumerable<XElement> data, String name)
    {
        DataTable table = new DataTable();
        table.TableName = name;
        
        //Adds Table headings based off data in the first row.         
        //Iterates through first XML tag to check for column names
        foreach (XAttribute attribute in data.First().Attributes())
        {
            table.Columns.Add(attribute.Name.ToString());
        }

        //Iterates through each row of the XML and adds it to the appropriate column
        foreach (XElement element in data)
        {
            var row = table.NewRow();
            for (int col = 0; col < table.Columns.Count; col++)
            {
                String colName = table.Columns[col].ColumnName;
                Console.WriteLine("element.Attribute(colName).Value " + element.Attribute(colName).Value);
                row[col] = element.Attribute(colName).Value;
            }
            table.Rows.Add(row);
        }
      
        return table;
    }
} 
