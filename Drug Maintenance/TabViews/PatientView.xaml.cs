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
using Drug_Maintenance.Persons;

/**********************************************/
using Drug_Maintenance;



namespace Drug_Maintenance.TabViews
{
    /// <summary>
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : UserControl
    {
        private ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
        private string required = "* Recuired";
        public PatientView()
        {
            InitializeComponent();

            patientsDataGrid.ItemsSource = patients;
            sourceCollectionUpdate();

            pRegDate.Text = DateTime.Now.ToString("dd : MMM : yyyy");
            pNicNo.MaxLength = 10;
            pContactNo.MaxLength = 10;
            gContactNo.MaxLength = 10;
        }

        
        private void disableItems()
        {
            this.wdNo.IsEnabled = false;

            this.pEnableCheck.IsEnabled = false;
            this.pBhtNo.IsEnabled = false;
            this.pNicNo.IsEnabled = false;
            this.pFirstName.IsEnabled = false;
            this.pMiddleName.IsEnabled = false;
            this.pLastName.IsEnabled = false;

            this.pDob.IsEnabled = false;
            this.pContactNo.IsEnabled = false;
            this.pStatus.IsEnabled = false;
            this.pAddr.IsEnabled = false;

            this.gName.IsEnabled = false;
            this.gRelation.IsEnabled = false;
            this.gContactNo.IsEnabled = false;

            this.pHistory.IsEnabled = false;
            this.pPrescription.IsEnabled = false;

            this.medicineDataGrid.IsEnabled = false;
        }

        private void enableItems()
        {
            this.wdNo.IsEnabled = true;

            this.pEnableCheck.IsEnabled = true;
            this.pBhtNo.IsEnabled = true;
            this.pNicNo.IsEnabled = true;
            this.pFirstName.IsEnabled = true;
            this.pMiddleName.IsEnabled = true;
            this.pLastName.IsEnabled = true;

            this.pDob.IsEnabled = true;
            this.pContactNo.IsEnabled = true;
            this.pStatus.IsEnabled = true;
            this.pAddr.IsEnabled = true;

            this.gName.IsEnabled = true;
            this.gRelation.IsEnabled = true;
            this.gContactNo.IsEnabled = true;

            this.pHistory.IsEnabled = true;
            this.pPrescription.IsEnabled = true;

            this.medicineDataGrid.IsEnabled = true;
        }

        private void clearFlags()
        {
            pBhtNoAlert.Content = "";
            pNameAlert.Content = "";
            pNicAlert.Content = "";
            pContactAlert.Content = "";
            pStatusAlert.Content = "";

            gNameAlert.Content = "";
            gRelationAlert.Content = "";
            gContactNoAlert.Content = "";
        }

        private int checker()
        {
            int check = 0;

            if (pBhtNo.Text == "")
            {
                check = 1;
                pBhtNoAlert.Content = required;
            }
            if (pFirstName.Text == "")
            {
                check = 1;
                pNameAlert.Content = required;
            }

            if (pNicNo.Text == "")
            {
                check = 1;
                pNicAlert.Content = required;
            }
            if (pContactNo.Text == "")
            {
                check = 1;
                pContactAlert.Content = required;
            }
            if (pStatus.SelectedIndex == -1)
            {
                check = 1;
                pStatusAlert.Content = required;
            }
            if (gName.Text == "")
            {
                check = 1;
                gNameAlert.Content = required;
            }

            if (gRelation.SelectedIndex == -1)
            {
                check = 1;
                gRelationAlert.Content = required;
            }
            if (gContactNo.Text == "")
            {
                check = 1;
                gContactNoAlert.Content = required;
            }

            return check;
        }

        private void clearItems()
        {
            this.pEnableCheck.IsChecked = true;

            this.pRegDate.Text = DateTime.Now.ToString("dd : MMM : yyyy");

            this.pBhtNo.Text = "";
            this.pNicNo.Text = "";
            this.pFirstName.Text = "";
            this.pMiddleName.Text = "";
            this.pLastName.Text = "";

            this.pDob.Text = "";
            this.pContactNo.Text = "";
            this.pStatus.SelectedIndex = -1;
            this.pAddr.Text = "";

            this.gName.Text = "";
            this.gRelation.SelectedIndex = -1;
            this.gContactNo.Text = "";

            this.pHistory.Text = "";
            this.pPrescription.Text = "";
        }

        private void disableButtons()
        {
            pEnableCheck.IsEnabled = false;
            pEnableCheck.IsChecked = false;
            addBtn.IsEnabled = false;
            editBtn.IsEnabled = false;
            saveBtn.IsEnabled = false;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            cancelBtn.IsEnabled = false;
        }

        private void enableButtons()
        {
            pEnableCheck.IsEnabled = true;
            addBtn.IsEnabled = true;
            editBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
            updateBtn.IsEnabled = true;
            deleteBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;
        }

        private void wdNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearItems();
            clearFlags();
            disableButtons();
            addBtn.IsEnabled = true;
            cancelBtn.IsEnabled = true;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

            disableButtons();
            saveBtn.IsEnabled = true;
            clearFlags();
            clearItems();
            enableItems();
            wdNo.IsEnabled = false;
            patientsDataGrid.SelectedIndex = -1;
            patientsDataGrid.IsEnabled = false;
            cancelBtn.IsEnabled = true;
        }
       
        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            int check = checker();
            if (check == 0)
            {
                try
                {
                    string validate = "select count(*) from patients where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year";

                    SqlCommand cmd = new SqlCommand(validate, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                    cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

                    sqlConnection.connection.Open();
                    Int32 count = (Int32)cmd.ExecuteScalar();
                    sqlConnection.connection.Close();

                    if (count == 0)
                    {
                        int enable = (this.pEnableCheck.IsChecked == true) ? 1 : 0;
                        string query = "insert into patients (pNicNo, pBhtNo, year, wdNo, pEnableCheck, pRegDate, pFirstName, pMiddleName, pLastName, pDob, pContactNo, pStatus, pAddr, gName, gRelation, gContactNo, pHistory, pPrescription) values (@pNicNo, @pBhtNo, @year, @wdNo, @pEnableCheck, @pRegDate, @pFirstName, @pMiddleName, @pLastName, @pDob, @pContactNo, @pStatus, @pAddr, @gName, @gRelation, @gContactNo, @pHistory, @pPrescription) ";

                        cmd.CommandText = query;
                        cmd.Connection = sqlConnection.connection;
                        cmd.Parameters.AddWithValue("@wdNo", int.Parse(this.wdNo.Text));
                        cmd.Parameters.AddWithValue("@pEnableCheck", enable);
                        cmd.Parameters.AddWithValue("@pRegDate", DateTime.Now.ToShortDateString());
                        cmd.Parameters.AddWithValue("@pFirstName", this.pFirstName.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@pMiddleName", this.pMiddleName.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@pLastName", this.pLastName.Text.ToUpper());

                        cmd.Parameters.AddWithValue("@pDob", this.pDob.Text);
                        cmd.Parameters.AddWithValue("@pContactNo", this.pContactNo.Text);
                        cmd.Parameters.AddWithValue("@pStatus", this.pStatus.Text);
                        cmd.Parameters.AddWithValue("@pAddr", this.pAddr.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@gName", this.gName.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@gRelation", this.gRelation.Text);
                        cmd.Parameters.AddWithValue("@gContactNo", this.gContactNo.Text);
                        cmd.Parameters.AddWithValue("@pHistory", this.pHistory.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@pPrescription", this.pPrescription.Text.ToUpper());


                        sqlConnection.connection.Open();
                        cmd.ExecuteNonQuery();

                        //showInfoDialog("Record has been Saved !");

                        string pName = null;

                        if (this.pMiddleName.Text == "")
                        {
                            pName = this.pFirstName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                        }
                        else
                        {
                            pName = this.pFirstName.Text.ToUpper() + " " + this.pMiddleName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                        }

                        //patients.Add(new Patient { pNicNo = this.pNicNo.Text, pBhtNo = int.Parse(this.pBhtNo.Text), year = int.Parse(DateTime.Now.Year.ToString()), wdNo = int.Parse(wdNo.Text.ToString()), pEnableCheck = enable, pName = pName });
                    }
                    else if (count == 1)
                    {
                        /*
                        var mySettings = new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "Yes",
                            NegativeButtonText = "No",
                            FirstAuxiliaryButtonText = "Cancel",
                            ColorScheme = MetroDialogOptions.ColorScheme
                        };

                        MessageDialogResult result = await this.ShowMessageAsync("Info Message!", "Record for the current patient has found !\nDo you want to Edit or View this Record ?",
                            MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                        if (result == MessageDialogResult.Affirmative)
                        {
                         */ 
                            string query = "select * from patients where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year ";

                            cmd.CommandText = query;
                            cmd.Connection = sqlConnection.connection;
                            cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                            cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                            cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

                            sqlConnection.connection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                           // readDB(reader);

                            disableButtons();
                            disableItems();
                            editBtn.IsEnabled = true;
                            deleteBtn.IsEnabled = true;

                            //patientsDataGrid.SelectedItem = patients.Where(x => x.pNicNo == this.pNicNo.Text && x.pBhtNo == int.Parse(this.pBhtNo.Text) && x.year == int.Parse(DateTime.Now.Year.ToString())).FirstOrDefault();
                            patientsDataGrid.IsEnabled = true;
                       // }

                    }
                }
                catch (Exception ex)
                {
                    string query = "delete from patients where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year ";

                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                    cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(ex.Message);
                    saveBtn.IsEnabled = true;
                }
                finally
                {
                    sqlConnection.connection.Close();
                }
            }
            cancelBtn.IsEnabled = true;
        }
        
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            clearItems();
            disableItems();
            wdNo.IsEnabled = true;
            wdNo.SelectedIndex = -1;
            patientsDataGrid.IsEnabled = true;
            patientsDataGrid.SelectedIndex = -1;
            disableButtons();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedIndex == -1)
            {
                enableItems();
            }
            else
            {
                addBtn.IsEnabled = false;
                editBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
                enableItems();
                patientsDataGrid.IsEnabled = false;

            }
        }

        private void readDB(SqlDataReader reader)
        {
            reader.Read();

            wdNo.SelectedIndex = int.Parse(reader["wdNo"].ToString().Trim()) - 1;

            pNicNo.Text = reader["pNicNo"].ToString().Trim();
            pBhtNo.Text = String.Format("{0:00000}", Convert.ToInt32(reader["pBhtNo"]));

            bool enab = Convert.ToInt32(reader["pEnableCheck"]) == 0 ? false : true;
            pEnableCheck.IsChecked = enab;

            DateTime regDate = Convert.ToDateTime(reader["pRegDate"]);
            pRegDate.Text = regDate.ToString("dd : MMM : yyyy");

            pFirstName.Text = reader["pFirstName"].ToString().Trim();
            pMiddleName.Text = reader["pMiddleName"].ToString().Trim();
            pLastName.Text = reader["pLastName"].ToString().Trim();

            pDob.SelectedDate = DateTime.Parse(reader["pDob"].ToString().Trim());

            pContactNo.Text = reader["pContactNo"].ToString().Trim();
            pStatus.SelectedValue = reader["pStatus"].ToString().Trim();

            pAddr.Text = reader["pAddr"].ToString().Trim();
            gName.Text = reader["gName"].ToString().Trim();
            gRelation.SelectedValue = reader["gRelation"].ToString().Trim();
            gContactNo.Text = reader["gContactNo"].ToString().Trim();

            pHistory.Text = reader["pHistory"].ToString().Trim();
            pPrescription.Text = reader["pPrescription"].ToString().Trim();

        }

        
        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
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
                 */ 
                    string query = "";

                    if (editBtn.IsEnabled == false || this.pNicNo.Text != "")
                    {
                        query = "delete from patients where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year";
                    }
                    else if (editBtn.IsEnabled == true)
                    {
                        Patient ePatient = patientsDataGrid.SelectedItem as Patient;
                        string nic = ePatient.pNicNo;
                        int ward = ePatient.wdNo;
                        string bht = ePatient.pBhtNo;
                       // int bht = ePatient.pBhtNo;
                        int year = ePatient.year;

                        query = "delete from patients where pNicNo='" + nic + "' and pBhtNo=" + bht + " and year=" + year + ";";
                    }

                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                    cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));


                    sqlConnection.connection.Open();

                    cmd.ExecuteNonQuery();

                    patients.Remove(patientsDataGrid.SelectedItem as Patient);
                //    showInfoDialog("Record has been deleted");

                    cancelBtn_Click(sender, e);
               // }
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
        
        private void colorChange_Click(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(((Button)sender).Content.ToString()), theme.Item1);
        }
        
        private async void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
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
                */
                    int enable = (this.pEnableCheck.IsChecked == true) ? 1 : 0;
                    string query = "update patients set pNicNo= @pNicNo, pBhtNo= @pBhtNo, year= @year, wdNo= @wdNo, pEnableCheck= @pEnableCheck,pRegDate= @pRegDate, pFirstName= @pFirstName, pMiddleName= @pMiddleName, pLastName= @pLastName, pDob= @pDob, pContactNo= @pContactNo, pStatus= @pStatus, pAddr= @pAddr, gName= @gName, gRelation= @gRelation, gContactNo= @gContactNo, pHistory= @pHistory, pPrescription= @pPrescription where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year ";

                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                    cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                    cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(this.pBhtNo.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));
                    cmd.Parameters.AddWithValue("@wdNo", int.Parse(wdNo.Text.ToString()));
                    cmd.Parameters.AddWithValue("@pEnableCheck", enable);
                    cmd.Parameters.AddWithValue("@pRegDate", DateTime.Now.ToShortDateString());

                    cmd.Parameters.AddWithValue("@pFirstName", this.pFirstName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@pMiddleName", this.pMiddleName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@pLastName", this.pLastName.Text.ToUpper());

                    cmd.Parameters.AddWithValue("@pDob", this.pDob.Text);
                    cmd.Parameters.AddWithValue("@pContactNo", this.pContactNo.Text);
                    cmd.Parameters.AddWithValue("@pStatus", this.pStatus.Text);
                    cmd.Parameters.AddWithValue("@pAddr", this.pAddr.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@gName", this.gName.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@gRelation", this.gRelation.Text);
                    cmd.Parameters.AddWithValue("@gContactNo", this.gContactNo.Text);
                    cmd.Parameters.AddWithValue("@pHistory", this.pHistory.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@pPrescription", this.pPrescription.Text.ToUpper());


                    sqlConnection.connection.Open();
                    cmd.ExecuteNonQuery();

                    updateBtn.IsEnabled = false;
                    disableItems();

                    patients.Remove(patientsDataGrid.SelectedItem as Patient);
                   // showInfoDialog("Record has been Updated !");

                    string pName = null;

                    if (this.pMiddleName.Text == "")
                    {
                        pName = this.pFirstName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                    }
                    else
                    {
                        pName = this.pFirstName.Text.ToUpper() + " " + this.pMiddleName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                    }

                   // patients.Add(new Patient { pNicNo = this.pNicNo.Text, pBhtNo = int.Parse(this.pBhtNo.Text), year = int.Parse(DateTime.Now.Year.ToString()), wdNo = int.Parse(wdNo.Text.ToString()), pEnableCheck = enable, pName = pName });
              //  }
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

        /*
        private async void showInfoDialog(string msg)
        {
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var controller = await this.ShowProgressAsync("Info Message", "\n\n" + msg);
            await Task.Delay(2750);
            await controller.CloseAsync();
        }
         */ 


        /*
           private Task ShowMessageAsync(string p1, string p2)
           {
               throw new NotImplementedException();
           }
         */

        
        private void sourceCollectionUpdate()
        {
            try
            {
                string query = "SELECT pNicNo, pBhtNo, year, wdNo, pEnableCheck, pFirstName, pMiddleName, pLastName FROM patients";

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
                DataSet patientDataSet = new DataSet();

                adapter.Fill(patientDataSet);

                foreach (DataRow row in patientDataSet.Tables[0].Rows)
                {
                    Patient patient = new Patient()
                    {
                        pNicNo = row[0].ToString().Trim(),
                        pBhtNo = String.Format("{0:00000}", row[1]),
                        year = int.Parse(row[2].ToString().Trim()),
                        wdNo = int.Parse(row[3].ToString().Trim()),
                        pEnableCheck = Convert.ToInt32(row[4]),
                        pName = (row[6] != System.DBNull.Value) ? row[5].ToString().Trim() + " " + row[6].ToString().Trim() + " " + row[7].ToString().Trim() : row[5].ToString().Trim() + " " + row[7].ToString().Trim()
                    };
                    patients.Add(patient);
                }

                // if the user is in wards, order by ward no, year, bht;
            }
            catch (Exception)
            {

            }
        }

        private void patientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (patientsDataGrid.SelectedIndex != -1)
            {

                try
                {
                    Patient ePatient = patientsDataGrid.SelectedItem as Patient;
                    string nic = ePatient.pNicNo;
                    string bht = ePatient.pBhtNo;
                    //int bht = ePatient.pBhtNo;
                    int year = ePatient.year;

                    string query = "select * from patients where pNicNo='" + nic + "' and pBhtNo=" + bht + " and year=" + year + ";";
                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);

                    sqlConnection.connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    //readDB(reader);
                    sqlConnection.connection.Close();

                    wdNo.IsEnabled = false;
                    addBtn.IsEnabled = false;
                    editBtn.IsEnabled = true;
                    deleteBtn.IsEnabled = true;
                    cancelBtn.IsEnabled = true;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void filterByTextInput(object sender, TextChangedEventArgs e)
        {
            try
            {
                cancelBtn.IsEnabled = true;
                TextBox t = (TextBox)sender;
                string filter = t.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(patientsDataGrid.ItemsSource);
                if (filter == "")
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        Patient p = o as Patient;
                        if (t.Name == "nicBox")
                        {
                            return (p.pNicNo.ToUpper().StartsWith(filter.ToUpper()));
                        }
                        if (t.Name == "bhtBox")
                        {
                            return (p.pBhtNo.EndsWith(filter));
                            //return (p.pBhtNo == Convert.ToInt32(filter));
                        }
                        if (t.Name == "yearBox")
                        {
                            return (p.year.ToString().StartsWith(filter));
                        }
                        return (p.pName.ToUpper().StartsWith(filter.ToUpper()));
                    };
                }
            }
            catch (FormatException)
            {

            }

        }

        private void filterBySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cancelBtn.IsEnabled = true;

                string filter = (e.AddedItems[0] as ComboBoxItem).Content as string;
                ICollectionView cv = CollectionViewSource.GetDefaultView(patientsDataGrid.ItemsSource);
                if (filter != "")
                {
                    cv.Filter = o =>
                    {
                        Patient p = o as Patient;
                        return (p.wdNo == Convert.ToInt32(filter));
                    };
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
        }

        private void filterByCheckBoxChecking(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as CheckBox).IsChecked.Value;
            int filter = (flag == true) ? 1 : 0;
            ICollectionView cv = CollectionViewSource.GetDefaultView(patientsDataGrid.ItemsSource);

            cv.Filter = o =>
            {
                Patient p = o as Patient;
                return (p.pEnableCheck == Convert.ToInt32(filter));
            };
        }

        
    }
}
