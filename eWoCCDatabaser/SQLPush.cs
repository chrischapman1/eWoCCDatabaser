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

        public void createTableQuery(DataTable dataTable, bool dropExisting)
        {
            //Checks if the datatable is already in the database and drops the existing table if necessary
            if ((checkIfDataTableInDataBase(dataTable.TableName)))
            {
                if (dropExisting)
                {
                   pushToSQL(new StringBuilder("DROP TABLE " + dataTable.TableName));
                }
            }

            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("CREATE TABLE " + dataTable.TableName + " ( ");

            //Checks if there are actually rows with data in them
            if (dataTable.Rows.Count == 0)
            {
                return;
            }

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
            //Avoids SQL exception: The number of row value expressions in the INSERT statement exceeds the maximum allowed number of 1000 row values.
            if (dataTable.Rows.Count > 1000)
            {
                double count = dataTable.Rows.Count;
                //Splits into groups of 500 to insert into SQL
                int numberToInsert = (int) Math.Ceiling(count / 500);

                var tables = dataTable.AsEnumerable().ToChunks(numberToInsert).Select(rows => rows.CopyToDataTable());
                foreach (var table in tables)
                {
                    table.TableName = dataTable.TableName;
                    insertToTable(table);
                }
            }
            
            //Checks that there is actually data in the dataTable
            if (dataTable.Rows.Count > 1)
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
                
                //Iterates through teh values to be inserted
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    sqlStatement.Append(" ( ");

                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        sqlStatement.Append("'");

                        sqlStatement.Append(dataTable.Rows[row].ItemArray[col]);

                        sqlStatement.Append("'");
                        sqlStatement.Append(", ");
                    }
                    sqlStatement.Remove(sqlStatement.Length - 2, 2);
                    sqlStatement.Append(" ) ");
                    sqlStatement.Append(", ");
                }
                sqlStatement.Remove(sqlStatement.Length - 2, 2);

                sqlStatement.Append(" ; ");

                //Debug
                Console.WriteLine("");
                Console.WriteLine(sqlStatement);
                Console.WriteLine("");

                pushToSQL(sqlStatement);
            }
        }

        public SqlConnection getSqlConnection()
        {
            String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\chris\Documents\HVCCC_eWoCC_DB.mdf;Integrated Security=True;Connect Timeout=30;";

            try
            {
                return new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Can not open connection to database: " + connectionString, e);
                return null;
            }

        }

        public void pushToSQL(StringBuilder query)
        {
            SqlCommand command;
            using (SqlConnection connection = getSqlConnection())
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
                    ErrorHandling.logError("Can not push query to the database: " + query.ToString().Substring(0, 300) + "... \n Full SQL statement can be found in the console. \n", e);
                }
                connection.Close();
            }
        
        }

        public Boolean checkIfDataTableInDataBase(String tableName)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM sys.tables WHERE [name]='");
            query.Append(tableName);
            query.Append("';");
            
            SqlCommand command;
            String testString ="";
            try
            {
                using (SqlConnection connection = getSqlConnection())
                {
                    connection.Open();
                    command = new SqlCommand(query.ToString(), connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == null)
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Can query database to check if " + tableName + "exists", e);
            }
            return false;
        }

        public String retrieveSQL(String query)
        {
            SqlCommand command;
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection connection = getSqlConnection())
                {
                    connection.Open();
                    command = new SqlCommand(query.ToString(), connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                if (reader.GetValue(i) != DBNull.Value)
                                    sb.AppendFormat("{0}-", Convert.ToString(reader.GetValue(i)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Error retrieving the query " + query, e);
            }
            return sb.ToString(); 
        }
    }
}
