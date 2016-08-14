using System;
using System.Collections.Generic;
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

using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

using MahApps.Metro.Controls;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Drug_Maintenance.Items;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.ComponentModel;

namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for LogsWindow.xaml
    /// </summary>
    public partial class LogsWindow : MetroWindow
    {

        private ObservableCollection<Request> stkGrid = new ObservableCollection<Request>();
        private ObservableCollection<Request> tranGrid= new ObservableCollection<Request>();
        public LogsWindow()
        {
            InitializeComponent();
            stockDataGrid.ItemsSource = stkGrid;
            transDataGrid.ItemsSource = tranGrid;

            loadStock();
            loadTransactions();
        }

        private void loadStock()
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
                r.request = stk.stockID.ToString().PadLeft(3, '0');
                stkGrid.Add(r);
            }
        }

        private void loadTransactions()
        {
            MongoClient client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("StockDatabase");
            var collection = db.GetCollection<Transactions>("TransactionCollection");

            var cursor = collection.FindAll();
            foreach (Transactions dep in cursor)
            {
                Request r = new Request();
                r.date = dep.date;
                r.request = dep.reqID;
                tranGrid.Add(r);
            }
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            stockFromError.Content = "";

            if (stockFrom.SelectedDate > stockTo.SelectedDate)
            {
                stockFromError.Content = "* Invalid Selection";
            }
            else
            {
                try
                {
                    ICollectionView cv = CollectionViewSource.GetDefaultView(stockDataGrid.ItemsSource);
                    
                    cv.Filter = o =>
                    {

                        Request r = o as Request;
                        return (r.date > stockFrom.SelectedDate &&  r.date < stockTo.SelectedDate);
                    };
                }
                catch (FormatException)
                {

                }
            }
        }

        private void pdfButton_Click(object sender, RoutedEventArgs e)
        {
            if (stockTab.IsSelected && stockDataGrid.SelectedIndex != -1) 
            {
                Request r = stockDataGrid.SelectedItem as Request;
                Stocks s = getStockMongo(Convert.ToInt32(r.request));

                ObservableCollection<Medicine> medi = new ObservableCollection<Medicine>();
                string resName = "SELECT uName FROM users WHERE uID=@uid;";
                SqlCommand c = new SqlCommand(resName, sqlConnection.connection);
                c.Parameters.AddWithValue("@uid", s.receiverID);

                try
                {
                    sqlConnection.connection.Open();
                    s.receiverName = (string)c.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                foreach (Medicine m in s.med)
                {
                    string getGenName = "SELECT id, genName, type FROM pharmaceuticals WHERE phID=@phID";
                    SqlCommand cmd = new SqlCommand(getGenName, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@phID", m.phID);
                    int id = 0;
                    string genName = "";
                    string type = "";
                    try
                    {
                        sqlConnection.connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        id = Convert.ToInt32(reader["id"]);
                        genName = reader["genName"].ToString().Trim();
                        type = reader["type"].ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.connection.Close();
                    }
                    m.id = id;
                    m.genName = genName;
                    m.type = type;
                    medi.Add(m);
                }

                genPDFStock(s);
            }
        }

        void genPDFStock(Stocks s)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Regular);
            XFont fontSubTitle = new XFont("Consolas", 16, XFontStyle.Regular);
            XFont fontRegular = new XFont("Consolas", 14, XFontStyle.Regular);
            XFont contentRegular = new XFont("Consolas", 14, XFontStyle.Regular);
            XFont tableTitle = new XFont("Consolas", 12, XFontStyle.Bold);
            XFont tableContent = new XFont("Consolas", 12, XFontStyle.Regular);

            // Draw the text
            gfx.DrawString("De Soyza Maternity Hospital", fontTitle, XBrushes.Black, new XRect(0, 40, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Stock Details", fontSubTitle, XBrushes.Black, new XRect(0, 70, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Stock ID : ", fontRegular, XBrushes.Black, new XRect(40, 110, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(s.stockID.ToString(), contentRegular, XBrushes.Black, new XRect(150, 110, page.Width, page.Height), XStringFormats.TopLeft);


            gfx.DrawString("Request ID : ", fontRegular, XBrushes.Black, new XRect(215, 110, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(s.requestID.ToString(), contentRegular, XBrushes.Black, new XRect(320, 110, page.Width, page.Height), XStringFormats.TopLeft);
            
            gfx.DrawString("Date : ", fontRegular, XBrushes.Black, new XRect(400, 110, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(s.stockDate.ToString("MM-dd-yyyy"), contentRegular, XBrushes.Black, new XRect(450, 110, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Receiver ID : ", fontRegular, XBrushes.Black, new XRect(40, 130, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(s.receiverID.ToString(), contentRegular, XBrushes.Black, new XRect(150, 130, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Receiver Name : ", fontRegular, XBrushes.Black, new XRect(215, 130, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(s.receiverName.ToString(), contentRegular, XBrushes.Black, new XRect(340, 130, page.Width, page.Height), XStringFormats.TopLeft);
            
            //Table Title
            gfx.DrawString("ID", tableTitle, XBrushes.Black, new XRect(40, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("NAME", tableTitle, XBrushes.Black, new XRect(80, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("TYPE", tableTitle, XBrushes.Black, new XRect(225, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("CATE.", tableTitle, XBrushes.Black, new XRect(280, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("DOSAGE", tableTitle, XBrushes.Black, new XRect(340, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("EXP. DATE", tableTitle, XBrushes.Black, new XRect(420, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("QTY.", tableTitle, XBrushes.Black, new XRect(520, 170, page.Width, page.Height), XStringFormats.TopLeft);

            int a = 0;
            foreach (Medicine m in s.med){
                gfx.DrawString(m.id.ToString(), tableContent, XBrushes.Black, new XRect(40, 190 + a, page.Width, page.Height), XStringFormats.TopLeft); 
                gfx.DrawString(m.genName, tableContent, XBrushes.Black, new XRect(80, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.type, tableContent, XBrushes.Black, new XRect(220, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(((m.category == "CONSUMABLE")?"C":"NC"), tableContent, XBrushes.Black, new XRect(290, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.dosage.ToString(), tableContent, XBrushes.Black, new XRect(350, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.expiryDate, tableContent, XBrushes.Black, new XRect(420, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.quantity.ToString(), tableContent, XBrushes.Black, new XRect(520, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                a +=15;
            }

            // Save the document...
            const string filename = "log.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        void genPDFTransactions(Transactions t)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Regular);
            XFont fontSubTitle = new XFont("Consolas", 16, XFontStyle.Regular);
            XFont fontRegular = new XFont("Consolas", 14, XFontStyle.Regular);
            XFont contentRegular = new XFont("Consolas", 13, XFontStyle.Regular);
            XFont tableTitle = new XFont("Consolas", 12, XFontStyle.Bold);
            XFont tableContent = new XFont("Consolas", 12, XFontStyle.Regular);

            // Draw the text
            gfx.DrawString("De Soyza Maternity Hospital", fontTitle, XBrushes.Black, new XRect(0, 40, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Transaction Details", fontSubTitle, XBrushes.Black, new XRect(0, 70, page.Width, page.Height), XStringFormats.TopCenter);

            gfx.DrawString("Transaction ID : ", fontRegular, XBrushes.Black, new XRect(40, 110, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(t.reqID.ToString(), contentRegular, XBrushes.Black, new XRect(200, 110, page.Width, page.Height), XStringFormats.TopLeft);

            /*
            gfx.DrawString("Request ID : ", fontRegular, XBrushes.Black, new XRect(215, 110, page.Width, page.Height), XStringFormats.TopLeft);
//            gfx.DrawString(s.requestID.ToString(), contentRegular, XBrushes.Black, new XRect(320, 110, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Date : ", fontRegular, XBrushes.Black, new XRect(400, 110, page.Width, page.Height), XStringFormats.TopLeft);
//            gfx.DrawString(s.stockDate.ToString("MM-dd-yyyy"), contentRegular, XBrushes.Black, new XRect(450, 110, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Receiver ID : ", fontRegular, XBrushes.Black, new XRect(40, 130, page.Width, page.Height), XStringFormats.TopLeft);
//            gfx.DrawString(s.receiverID.ToString(), contentRegular, XBrushes.Black, new XRect(150, 130, page.Width, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Receiver Name : ", fontRegular, XBrushes.Black, new XRect(215, 130, page.Width, page.Height), XStringFormats.TopLeft);
//            gfx.DrawString(s.receiverName.ToString(), contentRegular, XBrushes.Black, new XRect(340, 130, page.Width, page.Height), XStringFormats.TopLeft);
            */
 
            //Table Title
            gfx.DrawString("ID", tableTitle, XBrushes.Black, new XRect(40, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("NAME", tableTitle, XBrushes.Black, new XRect(80, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("TYPE", tableTitle, XBrushes.Black, new XRect(225, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("CATE.", tableTitle, XBrushes.Black, new XRect(280, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("DOSAGE", tableTitle, XBrushes.Black, new XRect(340, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("EXP. DATE", tableTitle, XBrushes.Black, new XRect(420, 170, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("QTY.", tableTitle, XBrushes.Black, new XRect(520, 170, page.Width, page.Height), XStringFormats.TopLeft);
            
            
            int a = 0;
            foreach (Medicine m in t.med)
            {
                gfx.DrawString(m.id.ToString(), tableContent, XBrushes.Black, new XRect(40, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.genName, tableContent, XBrushes.Black, new XRect(80, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.type, tableContent, XBrushes.Black, new XRect(220, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(((m.category == "CONSUMABLE") ? "C" : "NC"), tableContent, XBrushes.Black, new XRect(290, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.dosage.ToString(), tableContent, XBrushes.Black, new XRect(350, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.expiryDate, tableContent, XBrushes.Black, new XRect(420, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(m.quantity.ToString(), tableContent, XBrushes.Black, new XRect(520, 190 + a, page.Width, page.Height), XStringFormats.TopLeft);
                a += 15;
            }
            
            // Save the document...
            const string filename = "log.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        Stocks getStockMongo(int id)
        {
            MongoClient client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("StockDatabase");
            var collection = db.GetCollection<Stocks>("StockCollection");

            Stocks read = collection.FindOne(new QueryDocument { { "stockID", id } });
            return read;
        }

        Transactions getTransactionsMongo(string id)
        {
            MongoClient client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("StockDatabase");
            var collection = db.GetCollection<Transactions>("TransactionCollection");

            Transactions read = collection.FindOne(new QueryDocument { { "reqID", id } });
            return read;
        }

        private void logControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stockTab.IsSelected)
            {
                transDataGrid.SelectedIndex = -1;
            }
            else if (transTab.IsSelected)
            {
                stockDataGrid.SelectedIndex = -1;
            }
        }

        private void filterBtn_Trans_Click(object sender, RoutedEventArgs e)
        {
            stockFromError_Trans.Content = "";

            if (stockFrom_Trans.SelectedDate > stockTo_Trans.SelectedDate)
            {
                stockFromError_Trans.Content = "* Invalid Selection";
            }
            else
            {
                try
                {
                    ICollectionView cv = CollectionViewSource.GetDefaultView(transDataGrid.ItemsSource);

                    cv.Filter = o =>
                    {

                        Request r = o as Request;
                        return (r.date > stockFrom_Trans.SelectedDate && r.date < stockTo_Trans.SelectedDate);
                    };
                }
                catch (FormatException)
                {

                }
            }
        }

        private void pdfBtn_Click(object sender, RoutedEventArgs e)
        {
            if (transTab.IsSelected && transDataGrid.SelectedIndex != -1)
            {
                Request r = transDataGrid.SelectedItem as Request;
                Transactions t = getTransactionsMongo(r.request);

                ObservableCollection<Medicine> medi = new ObservableCollection<Medicine>();
                //string resName = "SELECT uName FROM users WHERE uID=@uid;";
                //SqlCommand c = new SqlCommand(resName, sqlConnection.connection);
                //c.Parameters.AddWithValue("@uid", s.receiverID);
                /*
                try
                {
                    sqlConnection.connection.Open();
                    s.receiverName = (string)c.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }
                */
                foreach (Medicine m in t.med)
                {
                    string getGenName = "SELECT id, genName, type FROM pharmaceuticals WHERE phID=@phID";
                    SqlCommand cmd = new SqlCommand(getGenName, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@phID", m.phID);
                    int id = 0;
                    string genName = "";
                    string type = "";
                    try
                    {
                        sqlConnection.connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        id = Convert.ToInt32(reader["id"]);
                        genName = reader["genName"].ToString().Trim();
                        type = reader["type"].ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.connection.Close();
                    }
                    m.id = id;
                    m.genName = genName;
                    m.type = type;
                    medi.Add(m);
                }
                t.med = medi;

                genPDFTransactions(t);
            }
        }
    }
}
