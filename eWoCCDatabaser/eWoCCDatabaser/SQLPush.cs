using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eWoCCDatabaser
{
    class SQLPush
    {
        public SQLPush() { }

        public void createTableQuery(DataTable dataTable)
        {
            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("CREATE TABLE " + dataTable.TableName + " ( ");

            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                sqlStatement.Append(dataTable.Columns[k].ColumnName);
                sqlStatement.Append(" ");
                bool isNumeric = false;
                bool usesColumnDefault = true;

                switch (dataTable.Columns[k].DataType.ToString().ToUpper())
                {
                    case "SYSTEM.INT16":
                        sqlStatement.Append(" smallint");
                        isNumeric = true;
                        break;
                    case "SYSTEM.INT32":
                        sqlStatement.Append(" int");
                        isNumeric = true;
                        break;
                    case "SYSTEM.INT64":
                        sqlStatement.Append(" bigint");
                        isNumeric = true;
                        break;
                    case "SYSTEM.DATETIME":
                        sqlStatement.Append(" datetime");
                        usesColumnDefault = false;
                        break;
                    case "SYSTEM.STRING":
                        sqlStatement.AppendFormat(" varchar(256)", dataTable.Columns[k].MaxLength);
                        break;
                    case "SYSTEM.SINGLE":
                        sqlStatement.Append(" single");
                        isNumeric = true;
                        break;
                    case "SYSTEM.DOUBLE":
                        sqlStatement.Append(" double");
                        isNumeric = true;
                        break;
                    case "SYSTEM.DECIMAL":
                        sqlStatement.AppendFormat(" decimal(18, 6)");
                        isNumeric = true;
                        break;
                    case "SYSTEM.BOOLEAN":
                        sqlStatement.AppendFormat(" bit");
                        break;
                    default:
                        sqlStatement.AppendFormat(" varchar(256)", dataTable.Columns[k].MaxLength);
                        break;
                }

                sqlStatement.Append(", ");
                
            }
            sqlStatement.Remove(sqlStatement.Length - 2, 2);
            sqlStatement.Append(" );");
            Console.WriteLine();
            Console.WriteLine(sqlStatement);
            Console.WriteLine();
            pushToSQL(sqlStatement);
        }

        public void insertToTable(DataTable dataTable)
        {
            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("INSERT INTO " + dataTable.TableName + " ( ");
            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                sqlStatement.Append(dataTable.Columns[k].ColumnName);
                sqlStatement.Append(" ");
                sqlStatement.Append(", ");
            }
            sqlStatement.Remove(sqlStatement.Length - 2, 2);
            sqlStatement.Append(" )");
            sqlStatement.Append(" VALUES ");
            for (int col = 0; col < dataTable.Columns.Count; col++) {
                sqlStatement.Append(" ( ");
                
                for (int row = 0; row < dataTable.Columns.Count; row++)
                {
                    sqlStatement.Append("'");

                    sqlStatement.Append(dataTable.Rows[row].ItemArray[col]);
                    
                    sqlStatement.Append("'");
                    sqlStatement.Append(", ");
                }
                sqlStatement.Remove(sqlStatement.Length - 2, 2);
                sqlStatement.Append(" ) ");
            }
            pushToSQL(sqlStatement);
        }

        public void pushToSQL(StringBuilder query)
        {
            System.Diagnostics.Debug.WriteLine(query.ToString());
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\chris\Documents\HVCCC_eWoCC_DB.mdf;Integrated Security=True;Connect Timeout=30;";
            //String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\CHRIS\DOCUMENTS\HVCCC_EWOCC_DB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlCommand command;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
        
                try
                {
                    command = new SqlCommand(query.ToString(), connection);
                    command.ExecuteReader();
                    command.Dispose();
                }
                catch (Exception e)
                {
                    ErrorHandling.logError("Can not open connection to database: " + connectionString, e);
                }
                connection.Close();
            }
        
        }
    }
}
