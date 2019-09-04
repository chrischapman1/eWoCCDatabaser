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
            StreamReader sr = new StreamReader(file);
            string[] headers = sr.ReadLine().Split(',');

            DataTable dt = new DataTable();
            file = file.Replace(".csv", "");
            int pos = file.LastIndexOf("/") + 1;
            file = file.Substring(pos, file.Length - pos);
            Console.WriteLine("FILENAME #### " + file);

            dt.TableName = file;

            foreach (string header in headers)
            {
                dt.Columns.Add(header);
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
