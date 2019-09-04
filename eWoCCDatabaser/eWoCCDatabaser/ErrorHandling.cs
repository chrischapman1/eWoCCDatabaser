using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    class ErrorHandling
    {
        public static void logError(String humanText, Exception e)
        {
            Console.WriteLine("Error Occured: " + humanText);
            Console.WriteLine("!!! Full Stack !!!");
            Console.WriteLine(e);
            System.Windows.Forms.MessageBox.Show(humanText + " Full stack trace can be found in Console", "eWoCC Databaser: Fatal Error");
        }
    }
}
