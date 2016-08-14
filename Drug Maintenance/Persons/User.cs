using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drug_Maintenance.Persons
{
    public class User
    {
        public string userID { get; set; }
        public string username { get; set; }
        public string uNicNo { get; set; }
        public int uEnableCheck { get; set; }
        public string uName { get; set; }
        public string uDep { get; set; }

    }


    public class Patient
    {
        public string pNicNo { get; set; }
        public string  pBhtNo { get; set; }
        public int year { get; set; }
        public int wdNo { get; set; }
        public int pEnableCheck { get; set; }
        public string pName { get; set; }

    }
}
