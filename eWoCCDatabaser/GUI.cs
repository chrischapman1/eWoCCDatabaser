using Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eWoCCDatabaser
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {

        }

        private MenuItem folderMenuItem, closeMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private string openFileName, folderName;
        private bool fileOpened = false, isPlurals = false;
        private OpenFileDialog openFileDialog1;
        private static string scenarioNameDate = "SENG4800Rail1_04122016";
        private List<DataTable> dataTableList;
        private List<String> postProcessingNames;
        private MenuItem fileMenuItem, openMenuItem;

        private void selectRootFolder_Click(object sender, EventArgs e)
        {
            dataTableList = new List<DataTable>();
            var fileContent = string.Empty;
            var filePath = string.Empty;
            string itemsToImport = "";

            this.folderMenuItem = new System.Windows.Forms.MenuItem();
            this.folderMenuItem.Text = "Select Directory...";
            this.folderMenuItem.Click += new System.EventHandler(this.selectRootFolder_Click);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog1.SelectedPath = "C:\\Users\\chris\\Documents\\University\\Semester 1 2019\\SENG4811 Final Year Project\\INDIV\\SENG4800_2019\\SENG4800Rail1\\1";

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;

                itemsToImport = modelInputsNames.Text;
                
                //Adds all of the modelInputs XML parameters
                importItems(itemsToImport, folderName);

                if (addEventLogs.Checked)
                {
                    //Adds all of the Event Log CSV files to DataTables and then into the database
                    String[] eventLogsProcessedOutputTables = getFileNames(folderName + "\\" + "EventLogs" + "\\" + "ProcessedOutput" + "\\" + "Tables" + "\\", ".csv");
                    folderToDatabase(eventLogsProcessedOutputTables);
                    
                }
                postProcess();
                System.Windows.Forms.MessageBox.Show("Completed processing. Added " + dataTableList.Count + " tables.");
            }

        }

        //Imports Model Inputs XML data
        private void importItems(String itemsToImport, String folderName)
        {
            List<string> items = itemsToImport.Replace(" ", "").Split(',').ToList<string>();
            XMLHelper xmlHelper = new XMLHelper();

            foreach (string current in items)
            {
                string searchTerm = "";
                if (addEventLogs.Checked)
                {
                    searchTerm = current.Replace("s", "");
                }
                else
                {
                    searchTerm = current;
                }
                Console.WriteLine("searchTerm " + searchTerm);
                DataTable dt = xmlHelper.XMLtoDataTable(folderName + "\\" + "_modelInputs.xml", searchTerm, scenarioNameDate);
                createDataTableToSQL(dt);
                dataTableToList(dt);
            }

        }

        //Accumulates a list of datatables which are used for post-processing
        private void dataTableToList(DataTable dt)
        {
            dataTableList.Add(dt);
        }

        //Creates the contents of csv in a folder into a database
        private void folderToDatabase(String[] fileUrls)
        {
            foreach (String current in fileUrls)
            {
                if (current.Contains("csv"))
                {
                    CSVHelper csvHelper = new CSVHelper();
                    DataTable dt = csvHelper.CSVtoDataTable(current);
                    createDataTableToSQL(dt);
                    dataTableToList(dt);
                }
            }
        }

        //SQL Query Helpers
        private void dataTableToSQL(DataTable dt)
        {
            SQLPush sqlPush = new SQLPush();
            sqlPush.insertToTable(dt);
        }

        private void createDataTableToSQL(DataTable dt)
        {
            SQLPush sqlPush = new SQLPush();
            sqlPush.createTableQuery(dt, true);
            sqlPush.insertToTable(dt);
        }

        //Retrieves desired schema name from user input
        private void schemaName_TextChanged(object sender, EventArgs e)
        {
            scenarioNameDate = schemaName.Text;
        }

        //Retrieves desired merges
        private void getPostProcessingNames()
        {
            String text = mergeData.Text;
            text.Replace(" ", "");
            postProcessingNames = text.Split(',').ToList();
        }

        //GUI Interfacing Methods
        private void isPlural_CheckedChanged(object sender, EventArgs e)
        {
            isPlurals = addEventLogs.Checked;
        }

        //Retrieves all of the files from EventLogs folder and sends them to the database. 
        private String[] getFileNames(String folderLocation, String fileType)
        {
            return Directory.GetFiles(folderLocation);
        }
        
        //Retrieves desired schema name
        public static String getScenarioNameDate()
        {
            return scenarioNameDate;
        }

        //Merges two datatables together, ignoring extra columns
        private DataTable mergeTwoDataTables(DataTable tableOne, DataTable tableTwo)
        {
            tableOne.TableName = tableOne.TableName.Substring(0, tableOne.TableName.IndexOf("_")) +
                                tableOne.TableName.Substring(0, tableOne.TableName.IndexOf("_")) +
                                "_" + getScenarioNameDate();
            tableOne.Merge(tableTwo, false, MissingSchemaAction.Ignore);
            Console.WriteLine(tableOne.TableName);
            return tableOne;
        }

        //Attempts to merge two datatables together using the list of post processing tables
        private void postProcess()
        {
            getPostProcessingNames();
            try
            {
                foreach (String current in postProcessingNames)
                {
                    foreach (DataTable dt in dataTableList)
                    {
                        if (current.Contains("_"))
                        {
                            //Removes the schema suffix
                            if ((dt.TableName.Substring(0, dt.TableName.IndexOf("_"))).Equals(current))
                            {
                                createDataTableToSQL(mergeTwoDataTables(dt, findDataTable(current)));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Could not find post processing data table. Do all of these tables " + postProcessingNames.ToString() + " exist?", e);
            }
        }

        //Queries the list of datatables and finds the name of the current one
        private DataTable findDataTable(String current)
        {
            foreach (DataTable dt in dataTableList)
            {
                //Removes the schema suffix
                if ((dt.TableName.Substring(0, dt.TableName.IndexOf("_"))).Equals(current))
                {
                    return dt;
                }
            }
            return null;
        }
    }
}
