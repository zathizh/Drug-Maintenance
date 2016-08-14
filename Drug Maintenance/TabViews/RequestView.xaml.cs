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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Drug_Maintenance;

namespace Drug_Maintenance.TabViews
{
    /// <summary>
    /// Interaction logic for RequestView.xaml
    /// </summary>
    public partial class RequestView : UserControl
    {
        public RequestView()
        {
            InitializeComponent();
        }

        private void disableRequestButtons() 
        {
            reqAddButton.IsEnabled = false;
            reqGenButton.IsEnabled = false;
            reqSubButton.IsEnabled = false;
        }

        private void enableRequestButtons()
        {
            reqAddButton.IsEnabled = true;
            reqGenButton.IsEnabled = true;
            reqSubButton.IsEnabled = true;
        }

        private void reqAddButton_Click(object sender, RoutedEventArgs e)
        {
            disableRequestButtons();
            reqGenButton.IsEnabled = true;
        }

        private void reqGenButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
