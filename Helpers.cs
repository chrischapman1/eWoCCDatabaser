using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWoCCDatabaser
{
    //Used for static helper functions that can be extended upon
    class Helpers
    {
        //Returns an input into a bit format
        public static int stringToBit(String input)
        {
            if (input.Equals("true"))
                return 1;
            else if (input.Equals("false"))
                return 0;
            else
                return -1;
        }
    }
}
