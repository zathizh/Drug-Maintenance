using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drug_Maintenance.Items;

using System.Collections.ObjectModel;
using System.Windows;

namespace Drug_Maintenance.Medi
{
    public class MediList : ObservableCollection<Medicine>
    {

        public MediList()
        {
            try
            {
                string query = "SELECT id, type, genName FROM pharmaceuticals;";

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
                DataSet medi = new DataSet();
                adapter.Fill(medi);
                foreach (DataRow row in medi.Tables[0].Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    Medicine meds = new Medicine()
                    {
                        //id = Convert.ToInt32(row[0]),
                        id = id,
                        type = row[1].ToString().Trim(),
                        //genName = row[2].ToString().Trim()
                        genName = row[2].ToString().Trim().PadRight(50, ' ') + id.ToString()
                    };
                    this.Add(meds);
 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
