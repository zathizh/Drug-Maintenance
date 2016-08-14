using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drug_Maintenance.Items;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Drug_Maintenance.Stock
{
    public class PharamaList : ObservableCollection<StockPharma>
    {
        public PharamaList() 
        {
            string query = "SELECT id, type, countable, category, genName FROM pharmaceuticals;";

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
                DataSet Pharma = new DataSet();

                try
                {
                    adapter.Fill(Pharma);
                }
                catch
                {

                }                
                foreach (DataRow row in Pharma.Tables[0].Rows)
                {
                    int id =  Convert.ToInt32(row[0]);
                    StockPharma pharma = new StockPharma()
                    {
                        //id = Convert.ToInt32(row[0]),
                        id = id,
                        type = row[1].ToString().Trim(),
                        countable = (Convert.ToInt32(row[2]) == 1) ? "COUNTABLE" : "NON-COUNTABLE",
                        category = row[3].ToString().Trim(),
                        //genName = row[4].ToString().Trim()
                        genName = row[4].ToString().Trim().PadRight(60, ' ') + id.ToString()

                    };
                    this.Add(pharma);
                }
        }
    }
}
