using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace eWoCCDatabaser
{
    class CSVHelper
    {
        public CSVHelper()
        {
            
        }

        //Sourced from: https://immortalcoder.blogspot.com/2013/12/convert-csv-file-to-datatable-in-c.html
        public DataTable CSVtoDataTable(String file)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
            }
            catch (IOException e)
            {
                ErrorHandling.logError("Please ensure all files are closed before importing", e);
            }

            string[] headers = sr.ReadLine().Split(',');

            DataTable dt = new DataTable();
            file = file.Replace(".csv", "");
            
            //This retrieves the last word of the URL (EG: the name of the file)
            file = file.Split((char)92)[file.Split((char)92).Length-1];
            
            dt.TableName = file + "_" + GUI.getScenarioNameDate();

            foreach (string header in headers)
            {
                dt.Columns.Add(header.Replace(" ", ""));
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
