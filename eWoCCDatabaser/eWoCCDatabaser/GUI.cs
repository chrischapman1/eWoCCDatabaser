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
            this.folderBrowserDialog1.SelectedPath =  "C:\\Users\\chris\\Documents\\University\\Semester 1 2019\\SENG4811 Final Year Project\\INDIV\\SENG4800_2019\\SENG4800Rail1\\1";
            
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;

                itemsToImport = modelInputsNames.Text;

                //Adds all of the modelInputs XML parameters
                importItems(itemsToImport, folderName);
                
                //Adds all of the Event Log CSV files to DataTables and then into the database
                //TODO: DELETE COMMENTS BELOW
                //String[] eventLogsProcessedOutputTables = getFileNames(folderName + "\\" + "EventLogs" + "\\" + "ProcessedOutput" + "\\" + "Tables" + "\\", ".csv");
                //folderToDatabase(eventLogsProcessedOutputTables);

                
            }

        }

        private void importItems(String itemsToImport, String folderName)
        {
            List<string> items = itemsToImport.Split(',').ToList<string>();
            XMLHelper xmlHelper = new XMLHelper();

            foreach (string current in items)
            {
                string searchTerm = "";
                if (isPlural.Checked)
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

        private void dataTableToList(DataTable dt)
        {
            dataTableList.Add(dt);
        }

        private void folderToDatabase(String[] fileUrls)
        {
            foreach (String current in fileUrls)
            {
                Console.WriteLine("current " + current);
                if (current.Contains("csv"))
                {
                    CSVHelper csvHelper = new CSVHelper();
                    createDataTableToSQL(csvHelper.CSVtoDataTable(current));
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

        private void schemaName_TextChanged(object sender, EventArgs e)
        {
            scenarioNameDate = schemaName.Text;
        }

        //GUI Interfacing Methods
        private void isPlural_CheckedChanged(object sender, EventArgs e)
        {
            isPlurals = isPlural.Checked;
        }

        //Retrieves all of the files from EventLogs folder and sends them to the database. 
        private String[] getFileNames(String folderLocation, String fileType)
        {
            return Directory.GetFiles(folderLocation);
        }

        public static String getScenarioNameDate()
        {
            return scenarioNameDate;
        }

        private DataTable mergeTwoDataTables(DataTable tableOne, DataTable tableTwo)
        {
            //return tableOne.Merge(tableTwo, false, MissingSchemaAction.Add);
            return null;
        }
    }
}
