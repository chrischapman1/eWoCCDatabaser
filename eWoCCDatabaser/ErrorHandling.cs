using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    class ErrorHandling
    {
        public static bool errorCreated = false;
        public static int id = 0;

        public static void logError(String humanText, Exception e)
        {
            Console.WriteLine("Error Occured: " + humanText);
            Console.WriteLine("!!! Full Stack !!!");
            Console.WriteLine(e);
            System.Windows.Forms.MessageBox.Show(humanText + " Full stack trace can be found in Console", "eWoCC Databaser: Error");
            createError(humanText, e);

        }

        public static void createError(String message, Exception e)
        {
            SQLPush sqlPush = new SQLPush();
            if (!(errorCreated))
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("id");
                dt.Columns.Add("error");
                dt.Columns.Add("exception");
                dt.Columns.Add("time");

                dt.TableName = "ErrorLogs";
                sqlPush.createTableQuery(dt, true);
                addToErrorTable(e, message, sqlPush);

            }
            else
            {
                addToErrorTable(e, message, sqlPush);
            }
            id++;
        }

        public static void addToErrorTable(Exception e, String message, SQLPush sqlPush)
        {
            DateTime dateTime = new DateTime();
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO ErrorLogs (id, error, exception, time) ");
            query.Append("VALUES ( " + id + " , " + message + " , " + "Exception message"  + " , " + dateTime.ToString() + ")");
            sqlPush.pushToSQL(query);
        }
    }
}
