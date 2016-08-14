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
using MahApps.Metro;
using MahApps.Metro.Controls;
using System.Data.SqlClient;
using MahApps.Metro.Controls.Dialogs;



namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private List<AccentColorMenuData> AccentColors { get; set; }
         
        public LoginWindow()
        {
            InitializeComponent();


            string formatDate = "dd MMM, yyyy";
            dateLabel.Content = DateTime.Now.ToString(formatDate);

            int t = int.Parse(DateTime.Now.ToString("HH"));
            if (t < 12)
            {
                greetingsLabel.Content = "Good Morning.";
            }
            else if (t < 14)
            {
                greetingsLabel.Content = "Good Afternoon.";
            }
            else
            {
                greetingsLabel.Content = "Good Evening.";
            }
        }

        public void ChangeAppStyle()
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(this);

            // now set the Red accent and dark theme
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent("Red"), ThemeManager.GetAppTheme("BaseDark"));
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(usernameField.Text)  && !String.IsNullOrEmpty(passwordField.Password))
            {
                string query = "SELECT COUNT(uDep) as count, uDep, userID FROM users WHERE username= @username and password= @password and uEnableCheck= @uEnableCheck GROUP BY uDep, userID;";
                    
                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@username", this.usernameField.Text);
                    cmd.Parameters.AddWithValue("@password", this.passwordField.Password);
                    cmd.Parameters.AddWithValue("@uEnableCheck", 1);

                    int count = -1, userDep = -1, userID = -1;

                    try
                    {
                        sqlConnection.connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        reader.Read();
                        count = Convert.ToInt32(reader["count"].ToString().Trim());
                        userDep = Convert.ToInt32(reader["uDep"].ToString().Trim());
                        userID = Convert.ToInt32(reader["userID"].ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.connection.Close();
                    }

                    if (count == 1)
                    {
                        MainWindow mainWindow = new MainWindow(userDep, userID);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        await this.ShowMessageAsync("Access Denied!", "Invalid Username or Password");
                    }
            }
            else
            {
                await this.ShowMessageAsync("Access Denied!", "Username and Password is Required");
            }
        }
    }
}
