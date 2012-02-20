using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myUserControls
{
    class FahrplanItem
    {
        private string linenumber;
        private string linename;
        private string linemin;


        public FahrplanItem(string linenumber, string linename, string linemin)
        {
            this.linenumber = linenumber;
            this.linename = linename;
            this.linemin = linemin;


        }


        public string Linenumber
        {
            get { return linenumber; }

        }

        public string Linename
        {
            get { return linename; }

        }

        public string Linemin
        {
            get { return linemin; }

        }
    }
}
