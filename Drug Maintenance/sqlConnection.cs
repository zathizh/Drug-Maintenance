using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Drug_Maintenance
{
    public static class sqlConnection
    {
        //public static SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\sathish\Documents\Visual Studio 2013\Projects\Drug Maintenance\Drug Maintenance\Hospital.mdf;Integrated Security=True");

        public static string conString = @"Data Source=DESKTOP-4960L0T\SQLEXPRESS;Initial Catalog=GP;Integrated Security=True";
        //public static SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-4960L0T\SQLEXPRESS;Initial Catalog=GP;Integrated Security=True");
        public static SqlConnection connection = new SqlConnection(conString);

        //public static string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\sathish\Documents\Visual Studio 2013\Projects\Drug Maintenance\Drug Maintenance\Hospital.mdf;Integrated Security=True";
        //public static SqlConnection connection = new SqlConnection(connectionString);

    }
}
