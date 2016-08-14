using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Drug_Maintenance.Items;
using Drug_Maintenance.Persons;
using System.Diagnostics;


namespace Drug_Maintenance.TabViews
{
    /// <summary>
    /// Interaction logic for StoreView.xaml
    /// </summary>
    public partial class StoreView : UserControl
    {

        ObservableCollection <Medicine> tables = new ObservableCollection<Medicine>(); 

        public StoreView()
        {
            InitializeComponent();
            viewStore.SelectedIndex = 0;
            viewStore.ItemsSource=tables;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearStoreButton.IsEnabled = true;
            if (viewDep.SelectedIndex != -1)
            {

                ComboBoxItem s = (ComboBoxItem)viewDep.SelectedItem;
                string d = s.Content.ToString();
                string dbView = null;

                switch (d)
                {
                    case "Store":
                        dbView = "storeView";
                        break;
                    case "Indoor Patient Department":
                        dbView = "ipdView";
                        break;
                    case "Outdoor Patient Department":
                        dbView = "opdView";
                        break;
                    case "Ward 01":
                        dbView = "wd01View";
                        break;
                    case "Ward 02":
                        dbView = "wd02View";
                        break;
                    case "Ward 03":
                        dbView = "wd03View";
                        break;
                    case "Ward 04":
                        dbView = "wd04View";
                        break;
                    case "Ward 05":
                        dbView = "wd05View";
                        break;
                    case "Ward 06":
                        dbView = "wd06View";
                        break;
                    case "Ward 07":
                        dbView = "wd07View";
                        break;
                    case "Ward 08":
                        dbView = "wd08View";
                        break;
                    case "Ward 09":
                        dbView = "wd09View";
                        break;
                    case "Ward 10":
                        dbView = "wd10View";
                        break;
                    case "Ward 11":
                        dbView = "wd11View";
                        break;
                    case "Ward 12":
                        dbView = "wd12View";
                        break;
                    case "Lab 01":
                        dbView = "lb01View";
                        break;
                    case "Lab 02":
                        dbView = "lb02View";
                        break;
                    case "Lab 03":
                        dbView = "lb03View";
                        break;
                    case "Theatre 01":
                        dbView = "th01View";
                        break;
                    case "Theatre 02":
                        dbView = "th02View";
                        break;
                    case "Theatre 03":
                        dbView = "th03View";
                        break;

                    default:
                        dbView = "storeView";
                        break;
                }

                    string query = null;

                    if (dbView == "storeView" || dbView == "ipdView")
                    {
                        query = "SELECT genName, type, dosage, (quantity-ISNULL(reduce, 0)), expiryDate FROM " + dbView + " WHERE (quantity-ISNULL(reduce, 0)) > 0;";
                    }
                    else
                    {
                        query = "SELECT genName, type, dosage, quantity, expiryDate FROM " + dbView + " ;";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
                    DataSet itemsDataSet = new DataSet();

                    try
                    {
                        adapter.Fill(itemsDataSet);
                    }
                    catch
                    {

                    }

                    tables.Clear();

                    foreach (DataRow row in itemsDataSet.Tables[0].Rows)
                    {
                        Medicine patient = new Medicine()
                        {
                            genName = row[0].ToString().Trim(),
                            type = row[1].ToString().Trim(),
                            dosage = Convert.ToInt32(row[2]),
                            quantity = Convert.ToInt32(row[3]),
                            expiryDate = Convert.ToDateTime(row[4]).ToShortDateString()
                        };
                        tables.Add(patient);
                    }
            }
        }

        private void nicBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(viewStore.ItemsSource);
                if (filter == "")
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        Medicine p = o as Medicine;
                        /*
                        if (t.Name == "genNameBox")
                        {
                            return (p.genName.ToUpper().StartsWith(filter.ToUpper()));
                        }
                         */ 
                        return (p.genName.ToUpper().StartsWith(filter.ToUpper()));
                    };
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearStoreButton_Click(object sender, RoutedEventArgs e)
        {
            viewDep.SelectedIndex = -1;
            tables.Clear();
            clearStoreButton.IsEnabled = false;
        }

    }
}
