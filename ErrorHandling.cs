using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    //Used to report errors to the user in a humanlike way
    class ErrorHandling
    {
        public static bool errorCreated = false;
        public static int id = 0;

        //Static method to log an error
        public static void logError(String humanText, Exception e)
        {
            Console.WriteLine("Error Occured: " + humanText);
            Console.WriteLine("!!! Full Stack !!!");
            Console.WriteLine(e);
            System.Windows.Forms.MessageBox.Show(humanText + ". Full stack trace can be found in Console", "eWoCC Databaser: Error");
            createError(humanText, e);
        }
        //Pushes the error to an SQL table
        public static void createError(String message, Exception e)
        {
            SQLPush sqlPush = new SQLPush();
            if (!(errorCreated))
            {
                //Creates datatable
                DataTable dt = new DataTable();

                dt.Columns.Add("id");
                dt.Columns.Add("error");
                dt.Columns.Add("exception");
                dt.Columns.Add("time");

                //pushes the data to MS SQL
                dt.TableName = "ErrorLogs";
                sqlPush.createTableQuery(dt, true);
                addToErrorTable(e, message, sqlPush);
                errorCreated = true;
            }
            else
            {
                addToErrorTable(e, message, sqlPush);
            }
            id++;
        }

        //INSERTS a single error to the prepopulated table
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
