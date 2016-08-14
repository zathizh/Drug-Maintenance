using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Drug_Maintenance.Items;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for StockWindow.xaml
    /// </summary>
    public partial class StockWindow : MetroWindow
    {
        private string required = "* Required";
        ObservableCollection<Medicine> mediItem = new ObservableCollection<Medicine>();
        //ObservableCollection<StockDetails> stockDetail = new ObservableCollection<StockDetails>();
        ObservableCollection<Request> stockDetail = new ObservableCollection<Request>();
        int user = -1, userID = -1;
        
        public StockWindow(int u, int uID)
        {
            user = u;
            userID = uID;
            InitializeComponent();

            date.Text = DateTime.Now.ToString("dd : MMM : yyyy");
            stockDataGrid.ItemsSource = mediItem;

            stockDetailDataGrid.ItemsSource = stockDetail;
            sourceCollectionUpdate();

            if (user != 15)
            {
                disableButtons();
            }

        }
        private void clearItems()
        {
            stockID.Text = "";
            requestID.Text = "";
            receiverID.Text = "";
            date.Text = "";

            mediItem.Clear();
        }
        private void enableItems()
        {
            stockID.IsEnabled = true;
            requestID.IsEnabled = true;
            //receiverID.IsEnabled = true;
            stockDataGrid.IsEnabled = true;
        }
        private void disableItems()
        {
            stockID.IsEnabled = false;
            requestID.IsEnabled = false;
            //receiverID.IsEnabled = false;
            stockDataGrid.IsEnabled = false;
        }
        private void enableButtons()
        {
            addBtn.IsEnabled = true;
            editBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
            updateBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;
        }
        private void disableButtons()
        {
            addBtn.IsEnabled = false;
            editBtn.IsEnabled = false;
            saveBtn.IsEnabled = false;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            cancelBtn.IsEnabled = false;
        }
        private int checker()
        {
            RegexUtilities util = new RegexUtilities();
            int check = 0;
            if (String.IsNullOrEmpty(receiverID.Text))
            {
                check = 1;
                receiverAlert.Content = required;
            }
            else if (!util.IsValidNumber(receiverID.Text))
            {
                check = 1;
                receiverAlert.Content = "Invalid ID";
            }

            return check;
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            saveBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;

            receiverID.Text = userID.ToString().PadLeft(4, '0');
            
            stockDetailDataGrid.IsEnabled = false;
            enableItems();
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            int check = checker();
            if (check == 0)
            {
                disableItems();

                ObservableCollection<Medicine> meds = new ObservableCollection<Medicine>();

                foreach (Medicine item in mediItem)
                {
                    string validate = "SELECT count(*) FROM store WHERE phID=@phID AND dosage=@dosage AND expiryDate=@expiryDate;";
                    SqlCommand cmd = new SqlCommand(validate, sqlConnection.connection);

                    meds.Add(new Medicine { phID = Convert.ToInt32(item.genName.ToString().Split().Last()), dosage = Convert.ToInt32(item.dosage), quantity = item.quantity, expiryDate = item.expiryDate.ToString() });

                    cmd.Parameters.AddWithValue("@phID", Convert.ToInt32(item.genName.ToString().Split().Last()));
                    cmd.Parameters.AddWithValue("@dosage", Convert.ToInt32(item.dosage));
                    cmd.Parameters.AddWithValue("@expiryDate", item.expiryDate.ToString());

                    Int32 count = -1;

                    try
                    {
                        sqlConnection.connection.Open();
                        count = (Int32)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.connection.Close();
                    }

                    if (count != -1)
                    {

                        if (count > 0)
                        {
                            string update = "UPDATE store SET quantity=quantity+@quantity WHERE phID=@phID AND dosage=@dosage AND expiryDate=@expiryDate;";
                            cmd.CommandText = update;
                        }
                        else if (count == 0)
                        {
                            string insert = "INSERT INTO store (phID, dosage, quantity, expiryDate, reduce) VALUES (@phID, @dosage, @quantity, @expiryDate, 0);";
                            cmd.CommandText = insert;
                        }

                        cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(item.quantity));

                        try
                        {
                            sqlConnection.connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            sqlConnection.connection.Close();
                        }
                    }
                }

                try
                {
                    MongoClient client = new MongoClient();
                    var server = client.GetServer();
                    var db = server.GetDatabase("StockDatabase");
                    var collection = db.GetCollection<Stocks>("StockCollection");

                    Stocks stk = new Stocks();

                    stk.med = meds;

                    stk.stockID = Convert.ToInt32(stockID.Text);
                    stk.receiverID = Convert.ToInt32(receiverID.Text);
                    stk.requestID = Convert.ToInt32(requestID.Text);

                    stk.stockDate = DateTime.Today;

                    collection.Save(stk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //stockDetail.Add(new StockDetails { stockID = stockID.Text + " " + DateTime.Today.ToString("MM-dd-yyyy") });
                stockDetail.Add(new Request { request = stockID.Text, date= DateTime.Today});
                showInfoDialog("Stock Added Successfully");
            }
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearItems();
            mediItem.Clear();
            disableButtons();
            addBtn.IsEnabled = true;

            stockDataGrid.IsEnabled = false;
            mediItem.Clear();
            stockDetailDataGrid.SelectedIndex = -1;
            stockDetailDataGrid.IsEnabled = true;
        }
        private async void showInfoDialog(string msg)
        {
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var controller = await this.ShowProgressAsync("Info Message", "\n\n" + msg);
            await Task.Delay(1750);
            await controller.CloseAsync();
        }
        private void sourceCollectionUpdate()
        {
            MongoClient client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("StockDatabase");
            var collection = db.GetCollection<Stocks>("StockCollection");

            var cursor = collection.FindAll();
            foreach (Stocks stk in cursor)
            {
                Request r = new Request();
                r.date = stk.stockDate;
                r.request = stk.stockID.ToString();
                stockDetail.Add(r);
            }
        }
        private void stockDetailDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stockDetailDataGrid.SelectedIndex != -1)
            {
                editBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
                cancelBtn.IsEnabled = true;

                clearItems();
                Request eStockDetail = stockDetailDataGrid.SelectedItem as Request;
                //StockDetails eStockDetail = stockDetailDataGrid.SelectedItem as StockDetails;
                try
                {
                    MongoClient client = new MongoClient();
                    var server = client.GetServer();
                    var db = server.GetDatabase("StockDatabase");
                    var collection = db.GetCollection<Stocks>("StockCollection");


                    int stkID = Convert.ToInt32(eStockDetail.request);
                    //int stkID = Convert.ToInt32(eStockDetail.stockID.Split().First());

                    Stocks stk = collection.FindOne(new QueryDocument { { "stockID", stkID } });

                    date.Text = stk.stockDate.ToString();

                    stockID.Text = stk.stockID.ToString();
                    requestID.Text = stk.requestID.ToString();
                    receiverID.Text = stk.receiverID.ToString();

                    foreach (Medicine eItem in stk.med)
                    {
                        string getGenName = "SELECT genName FROM pharmaceuticals WHERE phID=@phID";
                        SqlCommand cmd = new SqlCommand(getGenName, sqlConnection.connection);
                        cmd.Parameters.AddWithValue("@phID", eItem.phID);
                        string genName = "";

                        try
                        {
                            sqlConnection.connection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();
                            genName = reader["genName"].ToString().Trim();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            sqlConnection.connection.Close();
                        }

                        eItem.genName = genName;
                        mediItem.Add(eItem);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            disableItems();
            disableButtons();
            updateBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;

            stockDataGrid.IsEnabled = true;

        }
        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            //StockDetails eStockDetail = stockDetailDataGrid.SelectedItem as StockDetails;
            Request eStockDetail = stockDetailDataGrid.SelectedItem as Request;
            var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Cancel",
                    ColorScheme = MetroDialogOptions.ColorScheme
                };

            MessageDialogResult result = await this.ShowMessageAsync("Warrning!", "Do you want do delete this record ?",
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
            try
            {
                MongoClient client = new MongoClient();
                var server = client.GetServer();
                var db = server.GetDatabase("StockDatabse");
                var collection = db.GetCollection<Stocks>("StockCollection");

                int stkID = Convert.ToInt32(eStockDetail.request);
                //int stkID = Convert.ToInt32(eStockDetail.stockID.Split().First());

                collection.Remove(new QueryDocument { { "stockID", stkID } });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            showInfoDialog("Record has been deleted");
        }
        }
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<Medicine> meds = new ObservableCollection<Medicine>();

            try
            {
                MongoClient client = new MongoClient();
                var server = client.GetServer();
                var db = server.GetDatabase("StockDatabse");
                var collection = db.GetCollection<Stocks>("StockCollection");


                Stocks stk = new Stocks();

                stk.med = meds;

                stk.stockID = Convert.ToInt32(stockID.Text);
                stk.receiverID = Convert.ToInt32(receiverID.Text);
                stk.requestID = Convert.ToInt32(requestID.Text);

                stk.stockDate = DateTime.Today;

                collection.Remove(new QueryDocument { { "stockID", stk.stockID } });

                collection.Save(stk);

                MessageBox.Show("Successfully Updated the Stock !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            stockDataGrid.IsEnabled = false;
            
            stockDetail.Add(new Request { request = stockID.Text, date = DateTime.Today });
            //stockDetail.Add(new Request { StockID = stockID.Text + " " + DateTime.Today.ToString("MM-dd-yyyy") });
            showInfoDialog("Record updated Successfully");
        }
    }
}
