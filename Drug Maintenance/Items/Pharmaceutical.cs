using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Drug_Maintenance.Items
{
    public class Pharmaceutical
    {
        public string id { get; set; }
        public string type { get; set; }
        public int countable { get; set; }
        public string category { get; set; }
        public string genName { get; set; }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// INotifyPropertyChanged requires a property called PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the event for the property when it changes.
        /// </summary>
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
         
    }

    public class Stocks
    {
        public ObjectId Id { get; set; }
        public int stockID { get; set; }
        public int requestID { get; set; }
        public int receiverID { get; set; }
        public string receiverName { get; set; }
        public DateTime stockDate { get; set; }
        public ObservableCollection<Medicine> med { get; set; }

    }
    /*
    public class StockDetails
    {
        public string stockID { get; set; }
    }
     */ 

    /*
    public class Item
    {
        public int phID { get; set; }
        public string genName { get; set; }
        public string type { get; set; }
        public int dosage { get; set; }
        public int quantity { get; set; }
        public string expiryDate { get; set; }
    }
    */
    public class StockPharma
    {
        public int id { get; set; }
        public string type { get; set; }
        public string countable { get; set; }
        public string category { get; set; }
        public string genName { get; set; }
    }
    
    public class Meds 
    {
        public int phID { get; set; }
        public int id { get; set; }
        public string genName { get; set; }
        public int enable { get; set; }
        public int dosage { get; set; }
        public int frequency { get; set; }
        public int period { get; set; }
    }
    
    public class Medicine
    {
        public int phID { get; set; }
        public int id { get; set; }
        public string genName { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public int countable { get; set; }
        public int dosage { get; set; }
        public int quantity { get; set; }
        public string expiryDate { get; set; }
    }

    public class NotifyRequest 
    {
        public int phID { get; set; }
        public string genName { get; set; }
        public string type { get; set; }
        public int dosage { get; set; }
        public int quantity { get; set; }
        public int storedQuantity { get; set; }
        public string expiryDate { get; set; }
        public int issue { get; set; }

    }

    public class Request 
    {
        public DateTime date { get; set; }
        public String request { get; set; }
    }

    public class Transactions
    {
        public ObjectId Id { get; set; }
        public string reqID { get; set; }
        public DateTime date { get; set; }
        public ObservableCollection<Medicine> med { get; set; }
    }
}
