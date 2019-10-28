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
    //Acts as the beginning of this program and controls the GUI.
    public partial class GUI : Form
    {
        //Private member variables
        private MenuItem folderMenuItem, closeMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private string openFileName, folderName;
        private bool fileOpened = false, isPlurals = false;
        private OpenFileDialog openFileDialog1;
        private static string scenarioNameDate = "SENG4800Rail1_04122016";
        private List<DataTable> dataTableList;
        private List<String> postProcessingNames;
        private MenuItem fileMenuItem, openMenuItem;

        //Constructor
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {}

        //Main method which initiates everything
        //Result of selecting the root folder
        private void selectRootFolder_Click(object sender, EventArgs e)
        {
            //List of current data tables
            dataTableList = new List<DataTable>();
            var fileContent = string.Empty;
            var filePath = string.Empty;
            string itemsToImport = "";

            //File browser
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

                //If there are model inputs to import
                if (!(String.IsNullOrEmpty(modelInputsNames.Text)))
                {
                    itemsToImport = modelInputsNames.Text;
                    //Adds all of the modelInputs XML parameters
                    importItems(itemsToImport, folderName);
                }

                //If the event logs are to be imported
                if (addEventLogs.Checked)
                {
                    //Adds all of the Event Log CSV files to DataTables and then into the database
                    String[] eventLogsProcessedOutputTables = getFileNames(folderName + "\\" + "EventLogs" + "\\" + "ProcessedOutput" + "\\" + "Tables" + "\\", ".csv");
                    folderToDatabase(eventLogsProcessedOutputTables);
                }

                System.Windows.Forms.MessageBox.Show("Completed processing. Added " + dataTableList.Count + " tables.");

                //Begin post processing
                if ((!(String.IsNullOrEmpty(mergeData.Text))) && (postProcessBox.Checked == true))
                {
                    postProcess();
                    System.Windows.Forms.MessageBox.Show("Completed post processing. Added " + postProcessingNames.Count + " tables.");
                }
                
            }

        }

        //Imports Model Inputs XML data
        private void importItems(String itemsToImport, String folderName)
        {
            //List of items to import
            List<string> items = itemsToImport.Replace(" ", "").Split(',').ToList<string>();
            XMLHelper xmlHelper = new XMLHelper();

            //Iterates through all elements to import
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
                //Adds each of the items to the datatables
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

        //Iterates through a list of data tables and gets the desired one. Use in postprocessing.
        private DataTable getDataTable(String name)
        {
            foreach (DataTable dt in dataTableList)
            {
                if (dt.TableName.Contains(name))
                {
                    return dt;
                }
            }
            return new DataTable();
        }

        //Creates the contents of csv in a folder into a database
        //Can be extended to include other file types with relevant parsers
        private void folderToDatabase(String[] fileUrls)
        {
            foreach (String current in fileUrls)
            {
                if (current.Contains("csv"))
                {
                    
                    CSVHelper csvHelper = new CSVHelper();
                    //Retrieves the current file and puts it into a datatable
                    DataTable dt = csvHelper.CSVtoDataTable(current);
                    createDataTableToSQL(dt);
                    dataTableToList(dt);
                }
            }
        }

        //Puts all data from a datatable into the SQL
        private void dataTableToSQL(DataTable dt)
        {
            SQLPush sqlPush = new SQLPush();
            sqlPush.insertToTable(dt);
        }

        //Creates a SQL table from datatable information
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

        //Retrieves a list of post processing names
        private void getPostProcessingNames()
        {
            String text = mergeData.Text;
            if (text.Contains(","))
            {
                text.Replace(" ", "");
                postProcessingNames = text.Split(',').ToList();
            }
        }

        //Retrieves a list of post processing names in a string
        private string getPostProcessingNamesString()
        {
            string current = "";
            foreach (string name in postProcessingNames)
            {
                current = current + ", ";
            }
            return current;
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
                                tableTwo.TableName.Substring(0, tableTwo.TableName.IndexOf("_")) +
                                "_" + getScenarioNameDate();
            tableOne.Merge(tableTwo, false, MissingSchemaAction.Ignore);
            return tableOne;
        }

        //Attempts to merge two datatables together using the list of post processing tables
        private void postProcess()
        {
            getPostProcessingNames();
            
            int length = postProcessingNames.Count;

            try
            {
                //Iterates through all datatables to post process
                int i = 0;
                for (i = 0; i < length - 1; i++)
                {
                    DataTable dtOne = getDataTable(postProcessingNames[i]);
                    DataTable dtTwo = getDataTable(postProcessingNames[i + 1]);

                    //Ensures the column counts are the same
                    if (dtOne.Columns.Count == dtTwo.Columns.Count)
                    {
                        createDataTableToSQL(mergeTwoDataTables(dtOne, dtTwo));
                        System.Windows.Forms.MessageBox.Show("Post Processing: Created the new table with " + dtOne.TableName + " and " + dtTwo.TableName);


                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Could not find post processing data table. Do all of these tables " + postProcessingNames.ToString() + " exist?", e);
            }
        }
    }
}
