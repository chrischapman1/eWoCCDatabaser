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
            //Tries to open the document
            XDocument  doc = XDocument.Parse(System.IO.File.ReadAllText(input));
            var result = doc.Descendants(dataType);
            List<XElement> data = result.ToList();

            //Checks if there is more than one element. If so, goes for one child deep.
            //Recursion is too expensive
            try
            {
                //return (DataTable)XMLtoDataTableHelper(data, dataType , scenarioName);
                //Wrong condition
                if (data.Descendants().Count() <= 0)
                {
                    return (DataTable)XMLtoDataTableHelper(data, dataType , scenarioName);
                }
                else
                {
                    return (DataTable)XMLtoDataTableOneChild(data, data.ElementAt(0).Name.LocalName, scenarioName);
                }
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Error inputting XML into database", e);
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
        DataTable tempTable = new DataTable();
        table.TableName = name + "_" + scenarioName;

        //Gets the data for each of the parent ID. Note: Only takes the first value which is normally the id. 
        XAttribute attribute = data.First().Attributes().First();
        String currentCol = attribute.Name.ToString();
        String currentVal = attribute.Value.ToString();

        for (int i=0; i < data.Count(); i++)
        {
            //Gets the XML name of the child element
            String childXMLName = data.First().FirstNode.ToString();
            var start = childXMLName.IndexOf("<") + 1;
            childXMLName = childXMLName.Substring(start, childXMLName.IndexOf(" ") - start);

            //Gets the name of the current child table id
            currentVal = data.ElementAt(i).Attributes().First().Value;
            
            //Begins a temp table that will later be merged
            tempTable = XMLtoDataTableHelper(data.Elements(childXMLName), name + "_" + i, scenarioName);
            tempTable.Columns.Add(currentCol);
            //for each node, add the corresponding id to the table. 
            foreach (DataRow row in tempTable.Rows) 
            {
                row[currentCol] = currentVal;
            }
            //Merges temp table (containing child data) to the parent table
            table.Merge(tempTable);
            tempTable = null;
        }

        return table;
    }

    //This method is heavily based off the code found here: https://dotnetfiddle.net/wfo4Fz
    //Count: number of iterations through the child data. If -1, does not exist
    private DataTable XMLtoDataTableHelper(IEnumerable<XElement> data, String name, String scenarioName)
    {
        DataTable table = new DataTable();
        table.TableName = name + "_" + scenarioName;

        //Adds Table headings based off data in the first row.         
        //Iterates through first XML tag to check for column names
        foreach (XAttribute attribute in data.First().Attributes())
        {
            table.Columns.Add(attribute.Name.ToString());
        }

        //Iterates through each row of the XML and adds it to the appropriate column
        int current = 0;
        foreach (XElement element in data)
        {
            //Iterates through the rows of the XML and adds them to a new row in the datatable.
            var row = table.NewRow();
            for (int col = 0; col < table.Columns.Count; col++)
            {
                String colName = table.Columns[col].ColumnName;
                row[col] = element.Attribute(colName).Value;
            }
            table.Rows.Add(row);
        }
      
        return table;
    }


} 
