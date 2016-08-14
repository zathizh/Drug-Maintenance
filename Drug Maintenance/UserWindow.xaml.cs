using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Data.SqlClient;
using Drug_Maintenance.Persons;

namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    /// 
    public partial class UserWindow : MetroWindow
    {
        private string required = "* Recuired";
        private ObservableCollection<User> users = new ObservableCollection<User>();
        public UserWindow()
        {
            InitializeComponent();
            usersDataGrid.ItemsSource = users;
            sourceCollectionUpdate();
            uNicNo.MaxLength = 10;
            uContactNo.MaxLength = 10;

        }
        private int checker()
        {
            RegexUtilities util = new RegexUtilities();
            int check = 0;

            if (String.IsNullOrEmpty(userID.Text))
            {
                check = 1;
                uidAlert.Content = required;
            }
            else if (!util.IsValidNumber(userID.Text))
            {
                check = 1;
                uidAlert.Content = "Invalid ID";
            }


            if (String.IsNullOrEmpty(username.Text))
            {
                check = 1;
                usernameAlert.Content = required;
            }

            if (String.IsNullOrEmpty(password.Password))
            {
                check = 1;
                passwordAlert.Content = required;
            }

            if (String.IsNullOrEmpty(uName.Text))
            {
                check = 1;
                uNameAlert.Content = required;
            }
            else if (!util.IsValidName(uName.Text))
            {
                check = 1;
                uNameAlert.Content = "Invalid Name";
            }

            if (String.IsNullOrEmpty(uEmail.Text))
            {
                check = 1;
                uEmailAlert.Content = required;
            }
            else if (!util.IsValidEmail(uEmail.Text))
            {
                check = 1;
                uEmailAlert.Content = "Invalid Email Address";
            }

            if (String.IsNullOrEmpty(uNicNo.Text))
            {
                check = 1;
                uNicAlert.Content = required;
            }
            else if (!util.IsValidNICNumber(uNicNo.Text)) 
            {
                check = 1;
                uNicAlert.Content = "Invalid NIC Number";
            }

            if (String.IsNullOrEmpty(uContactNo.Text))
            {
                check = 1;
                uContactNoAlert.Content = required;
            }
            else if (!util.IsValidTeleNumber(uContactNo.Text))
            {
                check = 1;
                uContactNoAlert.Content = "Invalid Contact Number";
            }

            if (String.IsNullOrEmpty(uDep.Text))
            {
                check = 1;
                uDepAlert.Content = required;
            }
            return check;
        }
        private void clearFlags ()
        {
            uidAlert.Content = "";
            usernameAlert.Content = "";
            passwordAlert.Content = "";
            uNameAlert.Content = "";
            uEmailAlert.Content = "";
            uNicAlert.Content = "";
            uContactNoAlert.Content = "";
            uDepAlert.Content = "";
        }
        private void clearItems()
        {
            userID.Text = "";
            username.Text = "";
            password.Clear();
            uName.Text = "";
            uEmail.Text = "";
            uNicNo.Text = "";
            uContactNo.Text = "";
            uDep.Text = "";
            uDep.SelectedIndex = -1;
            uDep.SelectedIndex = -1;
        }
        private void enableItems()
        {
            userID.IsEnabled = true;
            username.IsEnabled = true;
            password.IsEnabled = true;
            uEnableCheck.IsEnabled = true;
            uName.IsEnabled = true;
            uEmail.IsEnabled = true;
            uNicNo.IsEnabled = true;
            uContactNo.IsEnabled = true;
            uDep.IsEnabled = true;
        }
        private void disableItems()
        {
            userID.IsEnabled = false;
            username.IsEnabled = false;
            password.IsEnabled = false;
            uEnableCheck.IsEnabled = false;
            uName.IsEnabled = false;
            uEmail.IsEnabled = false;
            uNicNo.IsEnabled = false;
            uContactNo.IsEnabled = false;
            uDep.IsEnabled = false;
        }
        private void enableButtons() 
        {
            uEnableCheck.IsEnabled = true;
            addBtn.IsEnabled = true;
            editBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
            updateBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;
        }
        private void disableButtons()
        {
            {
                uEnableCheck.IsEnabled = false;
                addBtn.IsEnabled = false;
                editBtn.IsEnabled = false;
                saveBtn.IsEnabled = false;
                updateBtn.IsEnabled = false;
                deleteBtn.IsEnabled = false;
                cancelBtn.IsEnabled = false;
            }
        }
        private void readDB(SqlDataReader reader)
        {
            reader.Read();

            userID.Text = String.Format("{0:0000}", Convert.ToInt32(reader["userID"]));
            username.Text = reader["username"].ToString().Trim();
            password.Password = "";
            bool enab = Convert.ToInt32(reader["uEnableCheck"]) == 0 ? false : true;
            uEnableCheck.IsChecked = enab;
            uName.Text = reader["uName"].ToString().Trim();
            uEmail.Text = reader["uEmail"].ToString().Trim();
            uNicNo.Text = reader["uNicNo"].ToString().Trim();
            uContactNo.Text = reader["uContactNo"].ToString().Trim();
            
            //uDep.SelectedValue = reader["uDep"].ToString().Trim();

        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            saveBtn.IsEnabled = true;
            clearFlags();
            clearItems();
            enableItems();
            uEnableCheck.IsChecked = true;
            cancelBtn.IsEnabled = true;
            usersDataGrid.SelectedIndex = -1;
            usersDataGrid.IsEnabled = false;
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            clearItems();
            disableItems();
            disableButtons();
            addBtn.IsEnabled = true;
            //users.Clear();
            usersDataGrid.SelectedIndex = -1;
            usersDataGrid.IsEnabled = true;

        }
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usersDataGrid.SelectedIndex == -1)
            {
                enableItems();
            }
            else
            {
                usersDataGrid.IsEnabled = false;
                addBtn.IsEnabled = false;
                editBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
                enableItems();   
            }
        }
        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            int check = checker();
            if (check == 0)
            {
                disableButtons();
                    string validate = "select count(*) from users where uNicNo= @uNicNo ";
                    
                    SqlCommand cmd = new SqlCommand(validate, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@uNicNo", this.uNicNo.Text);

                    Int32 count = -1;

                    try
                    {
                        sqlConnection.connection.Open();
                        count = (Int32) cmd.ExecuteScalar();
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
                        if (count == 0)
                        {
                            int enable = (this.uEnableCheck.IsChecked == true) ? 1 : 0;
                            string query = "insert into users (userID, username, password, uEnableCheck, uName, uEmail, uNicNo, uContactNo, uDep) values( @userID, @username, @password, @uEnableCheck, @uName, @uEmail, @uNicNo, @uContactNo, @uDep) ";

                            cmd.CommandText = query;
                            cmd.Connection = sqlConnection.connection;

                            UserManagement.userList choice;

                            int dep = 0;

                            if (Enum.TryParse(this.uDep.Text, out choice))
                            {
                                dep = (int)choice;
                            }

                            cmd.Parameters.AddWithValue("@userID", int.Parse(this.userID.Text));
                            cmd.Parameters.AddWithValue("@username", this.username.Text);
                            cmd.Parameters.AddWithValue("@password", password.Password);
                            cmd.Parameters.AddWithValue("@uEnableCheck", enable);
                            cmd.Parameters.AddWithValue("@uName", this.uName.Text.ToUpper());
                            cmd.Parameters.AddWithValue("@uEmail", this.uEmail.Text.ToLower());
                            cmd.Parameters.AddWithValue("@uContactNo", this.uContactNo.Text);
                            cmd.Parameters.AddWithValue("@uDep", dep);

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
                            showInfoDialog("Record has been Saved");

                            query = "select * from users where uNicNo= @uNicNo ";
                            cmd.CommandText = query;
                            clearItems();

                            try
                            {
                                sqlConnection.connection.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                readDB(reader);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                sqlConnection.connection.Close();
                            }

                            disableItems();

                            users.Add(new User { userID = String.Format("{0:0000}", this.userID.Text), username = this.username.Text, uNicNo = this.uNicNo.Text, uEnableCheck = enable, uName = this.uName.Text.ToUpper(), uDep = this.uDep.Text });
                            //users.Add(new User { userID = int.Parse(this.userID.Text), username = this.username.Text, uNicNo = this.uNicNo.Text, uEnableCheck = enable, uName = this.uName.Text.ToUpper(), uDep = this.uDep.Text });

                        }
                        else if (count == 1)
                        {
                            var mySettings = new MetroDialogSettings()
                            {
                                AffirmativeButtonText = "Yes",
                                NegativeButtonText = "No",
                                FirstAuxiliaryButtonText = "Cancel",
                                ColorScheme = MetroDialogOptions.ColorScheme
                            };

                            MessageDialogResult result = await this.ShowMessageAsync("Info Message!", "Record for the current patient has found !\nDo you want to edit this Record ?",
                                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                            if (result == MessageDialogResult.Affirmative)
                            {
                                string query = "select * from users where uNicNo = @uNicNo";

                                cmd.CommandText = query;
                                cmd.Connection = sqlConnection.connection;

                                try
                                {
                                    sqlConnection.connection.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    readDB(reader);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                finally
                                {
                                    sqlConnection.connection.Close();
                                }

                                disableButtons();
                                disableItems();
                                editBtn.IsEnabled = true;
                                deleteBtn.IsEnabled = true;

                                usersDataGrid.SelectedItem = users.Where(x => x.uNicNo == this.uNicNo.Text).FirstOrDefault();
                                usersDataGrid.IsEnabled = true;
                            }

                        }
                    }
            }
            cancelBtn.IsEnabled = true;
        }
        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
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
                    string query = "";

                    if (editBtn.IsEnabled == false || this.uNicNo.Text != "")
                    {
                        query = "delete from users where uNicNo= @uNicNo ";
                    }
                    else if(editBtn.IsEnabled == true )
                    {
                        User eUser = usersDataGrid.SelectedItem as User;
                        string nic = eUser.uNicNo;

                        query = "delete from users where uNicNo='" + nic + "';";
                    }

                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@uNicNo", this.uNicNo.Text);

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

                    users.Remove(usersDataGrid.SelectedItem as User);
                    showInfoDialog("Record has been deleted");
                    
                    cancelBtn_Click(sender, e);
                }

        }
        private async void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Cancel",
                    ColorScheme = MetroDialogOptions.ColorScheme
                };

                MessageDialogResult result = await this.ShowMessageAsync("Warrning!", "Do you want do update this record ?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                if (result == MessageDialogResult.Affirmative)
                {
                    User eUser = usersDataGrid.SelectedItem as User;

                    string query = "select uID from users where uNicNo=@euNicNo";
                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@euNicNo", eUser.uNicNo);

                    Int32 uID = -1;

                    try
                    {
                        sqlConnection.connection.Open();
                        uID = (Int32)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.connection.Close();
                    }

                    if (uID != -1)
                    {

                        query = null;
                        int enable = (this.uEnableCheck.IsChecked == true) ? 1 : 0;

                        UserManagement.userList choice;
                        int dep = 0;

                        if (Enum.TryParse(this.uDep.Text, out choice))
                        {
                            dep = (int)choice;
                        }

                        if (this.password.Password == "")
                        {
                            query = "update users set userId= @userId, username= @username, uEnableCheck= @uEnableCheck, uName= @uName, uEmail= @uEmail, uNicNo= @uNicNo, uContactNo= @uContactNo, uDep= @uDep where uNicNo= @uID ";
                        }
                        else
                        {
                            query = "update users set userId= @userId, username= @username, password= @password, uEnableCheck= @uEnableCheck, uName= @uName, uEmail= @uEmail, uNicNo= @uNicNo, uContactNo= @uContactNo, uDep= @uDep where uNicNo= @uID ";
                        }

                        cmd.Parameters.AddWithValue("@uID", uID);
                        cmd.Parameters.AddWithValue("@userId", int.Parse(this.userID.Text));
                        cmd.Parameters.AddWithValue("@username", this.username.Text);
                        cmd.Parameters.AddWithValue("@password", this.password.Password);
                        cmd.Parameters.AddWithValue("@uEnableCheck", enable);
                        cmd.Parameters.AddWithValue("@uName", this.uName.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@uEmail", this.uEmail.Text.ToLower());
                        cmd.Parameters.AddWithValue("@uNicNo", this.uNicNo.Text);
                        cmd.Parameters.AddWithValue("@uContactNo", this.uContactNo.Text);
                        cmd.Parameters.AddWithValue("@uDep", dep);

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

                        updateBtn.IsEnabled = false;
                        disableItems();

                        users.Remove(usersDataGrid.SelectedItem as User);
                        showInfoDialog("Record has been Updated");

                        users.Add(new User { userID = String.Format("{0:0000}", this.userID.Text), username = this.username.Text.ToUpper(), uNicNo = this.uNicNo.Text, uEnableCheck = enable, uName = this.uName.Text, uDep = this.uDep.Text });
                        //users.Add(new User { userID = int.Parse(this.userID.Text), username = this.username.Text.ToUpper(), uNicNo = this.uNicNo.Text, uEnableCheck = enable, uName = this.uName.Text, uDep = this.uDep.Text });

                        usersDataGrid.IsEnabled = true;
                        addBtn.IsEnabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection.connection.State == ConnectionState.Open)
                {
                    sqlConnection.connection.Close();
                }
            }
        }
        private async void showInfoDialog(string msg)
        {
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var controller = await this.ShowProgressAsync("Info Message", "\n\n" + msg);
            await Task.Delay(2750);
            await controller.CloseAsync();
        }
        /*
        private Task ShowMessageAsync(string p1, string p2)
        {
            throw new NotImplementedException();
        }
         * */
        private void sourceCollectionUpdate() 
        {
            string query = "SELECT userID, username, uNicNo, uEnableCheck, uName,uDep FROM users";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
            DataSet userDataSet = new DataSet();

            try
            {
                adapter.Fill(userDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (DataRow row in userDataSet.Tables[0].Rows)
            {
                User user = new User()
                {
                    userID = String.Format("{0:0000}", row[0]),
                    username = row[1].ToString().Trim(),
                    uNicNo = row[2].ToString().Trim(),
                    uEnableCheck = Convert.ToInt32(row[3]),
                    uName = row[4].ToString().Trim(),
                    uDep =  Enum.GetName(typeof(UserManagement.userList), Convert.ToInt32(row[5].ToString().Trim()))
                };

                users.Add(user);
            }
        }
        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersDataGrid.SelectedIndex != -1)
            {
                User eUser = usersDataGrid.SelectedItem as User;
                string nic = eUser.uNicNo;

                string query = "select * from users where uNicNo= @uNicNo";
                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@uNicNo", nic);

                try
                {
                    sqlConnection.connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    readDB(reader);
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                editBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
                cancelBtn.IsEnabled = true;
            }
        }
        private void filterByTextInput(object sender, TextChangedEventArgs e)
        {
            try
            {
                cancelBtn.IsEnabled = true;
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(usersDataGrid.ItemsSource);
                if (filter == "")
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        User p = o as User;
                        if (t.Name == "idBox")
                        {
                            return (p.userID.EndsWith(filter));
                        }
                        if (t.Name == "usernameBox")
                        {
                            return (p.username.ToUpper().StartsWith(filter.ToUpper()));
                        }
                        if (t.Name == "nicBox")
                        {
                            return (p.uNicNo.ToUpper().StartsWith(filter.ToUpper()));
                        }
                        if (t.Name == "nameBox")
                        {
                            return (p.uName.ToUpper().StartsWith(filter.ToUpper()));
                        }
                        return (p.uDep.ToUpper().StartsWith(filter.ToUpper()));
                    };
                }
            }
            catch (FormatException) { }
        }
        private void filterByCheckBoxChecking(object sender, RoutedEventArgs e)
        {
            cancelBtn.IsEnabled = true;
            bool flag = (sender as CheckBox).IsChecked.Value;
            int filter = (flag == true) ? 1 : 0;
            ICollectionView cv = CollectionViewSource.GetDefaultView(usersDataGrid.ItemsSource);
            
            cv.Filter = o =>
            {
                User p = o as User;
                return (p.uEnableCheck == Convert.ToInt32(filter));
            };
        }
    }
}
