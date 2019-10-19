﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    class ErrorHandling
    {
        private bool errorCreated = false;
        private int id = 0;

        public static void logError(String humanText, Exception e)
        {
            Console.WriteLine("Error Occured: " + humanText);
            Console.WriteLine("!!! Full Stack !!!");
            Console.WriteLine(e);
            System.Windows.Forms.MessageBox.Show(humanText + " Full stack trace can be found in Console", "eWoCC Databaser: Fatal Error");

            //Probably a bad idea but you get many message boxes otherwise
            //System.Environment.Exit(1);
        }

        public void createErrorTableAndPush(String message, Exception e)
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
                
            }
            else
            {
                addToErrorTable(e, message, sqlPush);
            }
            id++;
        }

        public void addToErrorTable(Exception e, String message, SQLPush sqlPush)
        {
            DateTime dateTime = new DateTime();
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO ErrorLogs (id, error, exception, time) ");
            query.Append("VALUES ( " + id + " , " + message + " , " + e.ToString() + " , " + dateTime.ToString() + ")");
            sqlPush.pushToSQL(query);
        }
    }
}
