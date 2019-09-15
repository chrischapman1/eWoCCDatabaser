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
        private bool fileOpened = false, addEventLogs = false;
        private OpenFileDialog openFileDialog1;
        private static string scenarioNameDate;

        private MenuItem fileMenuItem, openMenuItem;

        private void selectRootFolder_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            
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

                XMLHelper modelInputs = new XMLHelper(folderName + "\\" + "_modelInputs.xml");

                scenarioNameDate = findScenarioInformation(modelInputs.getDataObject().runParameters);

                //CAN REMOVE
               // DataTable dt = csvHelper.CSVtoDataTable(folderName + "\\" + "EventLogs" + "\\" + "ProcessedOutput" + "\\" + "Tables" + "\\" + "vesselQ.csv");
                //createDataTableToSQL(dt);

                //LEAVE
                //dataTableToSQL(runParameters(modelInputs.getDataObject().runParameters));

               
                String[] eventLogsProcessedOutputTables = getFileNames(folderName + "\\" + "EventLogs" + "\\" + "ProcessedOutput" + "\\" + "Tables" + "\\", ".csv");

                folderToDatabase(eventLogsProcessedOutputTables);


                //DataTable dt = addGenericData(modelInputs.getDataObject().channelDistances[0].channelDistance);

                // DataTable dt = runParameters(modelInputs.getDataObject().runParameters);

            }

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

        //Naming

        private string findScenarioInformation(dataRunParameters[] runParameters)
        {
            return runParameters[0].runParameter[0].value + "_" + runParameters[0].runParameter[1].value;
        }
     

        //RunParameters has differing data types, so it must be implemented manually. 
        private DataTable runParameters(dataRunParameters[] runParameters)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "RunParameters" + "_" + scenarioNameDate.Replace("/","");

                dt.Columns.Add(runParameters[0].runParameter[0].name, typeof(String)); //scenarioID
                dt.Columns.Add(runParameters[0].runParameter[1].name, typeof(String)); //runStartDate --> not date time?
                dt.Columns.Add(runParameters[0].runParameter[2].name, typeof(Boolean)); //endOnDemandComplete

                //All of type Integer
                for (int i = 3; i <= 6; i++)
                {
                    dt.Columns.Add(runParameters[0].runParameter[i].name, typeof(Int32));
                }

                //All of type 
                for (int i = 7; i < 12; i++)
                {
                    dt.Columns.Add(runParameters[0].runParameter[i].name, typeof(Boolean));
                }


                //Adds data to rows
                DataRow row = dt.NewRow();

                for (int i = 0; i < 12; i++)
                {
                    row[runParameters[0].runParameter[i].name] = Helpers.stringToBit(runParameters[0].runParameter[i].value);
                }

                dt.Rows.Add(row);
                return dt;
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Your run parameters are not set up correctly, have you added or removed any?" , e);
                return null;
            }
            
        }

        

        private DataTable addGenericData(Object[] o)
        {
            DataTable dt = new DataTable();
            dt.TableName = o.GetType() + "_" + scenarioNameDate.Replace("/", "");

            for (int count = 0; count < o.Length; count++)
            {
                dt.Columns.Add(o.ToString(), typeof(String));
            }
            for (int count = 0; count < o.Length; count++)
            {

            }

            return dt;
        }

        //Interface for Data with only boolean inputs 
        private static DataTable dataTableBoolean<T>(T xmlData) where T : IXMLDataBoolean
        {
            try
            {
                DataTable dt = new DataTable();
               // dt.TableName = "RunParameters" + "_" + GUI.scenarioNameDate.Replace("/", "");

               // dt.Columns.Add(runParameters[0].runParameter[0].name, typeof(String)); //scenarioID
                
                //Adds data to rows
                DataRow row = dt.NewRow();

                for (int i = 0; i < 12; i++)
                {
                //    row[runParameters[0].runParameter[i].name] = Helpers.stringToBit(runParameters[0].runParameter[i].value);
                }

                dt.Rows.Add(row);
                return dt;
            }
            catch (Exception e)
            {
                ErrorHandling.logError("Your run parameters are not set up correctly, have you added or removed any?", e);
                return null;
            }

        }

        //GUI Interfacing Methods

        private void addEventLogs_CheckedChanged(object sender, EventArgs e)
        {
            addEventLogs = addEventLogsBox.Checked;
        }

        //Getters and Setters

        public static string getScenarioNameDate()
        {
            scenarioNameDate = scenarioNameDate.Replace("/", "");
            return scenarioNameDate;
        }

        //Retrieves all of the files from EventLogs folder and sends them to the database. 
        private String[] getFileNames(String folderLocation, String fileType)
        {
            return Directory.GetFiles(folderLocation);
        }

    }
}
