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
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Data.SqlClient;
using System.Data;
using Drug_Maintenance.Persons;
using Drug_Maintenance.Items;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;


namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
        private ObservableCollection<Meds> med = new ObservableCollection<Meds>();
        private ObservableCollection<Medicine> request = new ObservableCollection<Medicine>();

        private ObservableCollection<NotifyRequest> notify = new ObservableCollection<NotifyRequest>();

        private ObservableCollection<Request> requestList = new ObservableCollection<Request>();
        private ObservableCollection<Request> req = new ObservableCollection<Request>();
        private ObservableCollection<Request> con = new ObservableCollection<Request>();

        private readonly MainWindowViewModel _viewModel;

        private PharmaceuticalWindow pw;
        private UserWindow uw;
        private AboutWindow aw;
        private StockWindow sw;
        private LogsWindow lw;

        private string required = "* Recuired";
        //private Task ShowMessageAsync;

        public int user = -1, userID;

        public MainWindow(int u, int uID)
        {
            user = u;
            userID = uID;

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            InitializeComponent();

            patientsDataGrid.ItemsSource = patients;
            medicineDataGrid.ItemsSource = med;
            requestDataGrid.ItemsSource = request;
            reqDataNotifyDataGrid.ItemsSource = notify;

            notifReqDataGrid.ItemsSource = requestList;

            sourceCollectionUpdate();
            requestNotifier();


            if (user <= 12)
            {
                wdNo.SelectedIndex = user - 1;

                stock.IsEnabled = false;
                pharmaceuticals.IsEnabled = false;
                users.IsEnabled = false;
            }
            else if (user <= 14)
            {
                users.IsEnabled = false;
            }
            else if (user == 15)
            {
                //wdNo.SelectedIndex = user - 1;
                requestTab.IsEnabled = false;
                users.IsEnabled = false;
            }
            else if (user == 16)
            {

            }

            wdNo.IsEnabled = false;

            pRegDate.Text = DateTime.Now.ToString("dd : MMM : yyyy");
            pNicNo.MaxLength = 10;
            pContactNo.MaxLength = 10;
            gContactNo.MaxLength = 10;

            requestNotifier();
            confirmNotifier();
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
            pAddrAlert.Content = "";

            gNameAlert.Content = "";
            gRelationAlert.Content = "";
            gContactNoAlert.Content = "";
        }

        private int checker()
        {
            RegexUtilities util = new RegexUtilities();
            int check = 0;

            if (String.IsNullOrEmpty(pBhtNo.Text))
            {
                check = 1;
                pBhtNoAlert.Content = required;
            }
            else if (!util.IsValidNumber(pBhtNo.Text))
            {
                check = 1;
                pBhtNoAlert.Content = "Invalid BHT Number";
            }

            if (String.IsNullOrEmpty(pFirstName.Text) && String.IsNullOrEmpty(pLastName.Text))
            {
                check = 1;
                pNameAlert.Content = required;
            }
            else if (String.IsNullOrEmpty(pFirstName.Text))
            {
                check = 1;
                pNameAlert.Content = "Frist Name Required";
            }
            else if (String.IsNullOrEmpty(pLastName.Text))
            {
                check = 1;
                pNameAlert.Content = "Last Name Required";
            }
            else if (!util.IsValidName(pFirstName.Text) || !util.IsValidName(pLastName.Text))
            {
                check = 1;
                pNameAlert.Content = "Invalid Name";
            }
            else if (!String.IsNullOrEmpty(pMiddleName.Text) && !util.IsValidName(pMiddleName.Text))
            {
                check = 1;
                pNameAlert.Content = "Invalid Name";

            }
            if (!util.IsValidNICNumber(pNicNo.Text))
            {
                check = 1;
                pNicAlert.Content = "Invalid NIC";
            }

            if (String.IsNullOrEmpty(pContactNo.Text))
            {
                check = 1;
                pContactAlert.Content = required;
            }
            else if (!util.IsValidTeleNumber(pContactNo.Text))
            {
                check = 1;
                pContactAlert.Content = "Invalid Contact Number";
            }

            if (pStatus.SelectedIndex == -1)
            {
                check = 1;
                pStatusAlert.Content = required;
            }

            if (!util.IsValidAddress(pAddr.Text))
            {
                check = 1;
                pAddrAlert.Content = "Invalid Address";
            }

            if (String.IsNullOrEmpty(gName.Text))
            {
                check = 1;
                gNameAlert.Content = required;
            }
            else if (!util.IsValidName(gName.Text))
            {
                check = 1;
                gNameAlert.Content = "Invalid Name";
            }

            if (gRelation.SelectedIndex == -1)
            {
                check = 1;
                gRelationAlert.Content = required;
            }
            if (String.IsNullOrEmpty(gContactNo.Text))
            {
                check = 1;
                gContactNoAlert.Content = required;
            }
            else if (!util.IsValidTeleNumber(gContactNo.Text))
            {
                check = 1;
                gContactNoAlert.Content = "Invalid Contact Number";
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

            this.pDob.SelectedDate = DateTime.Now;
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
            medEditButton.IsEnabled = false;
            saveBtn.IsEnabled = false;
            updateBtn.IsEnabled = false;
            medUpdateButton.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            cancelBtn.IsEnabled = false;
        }

        private void enableButtons()
        {
            pEnableCheck.IsEnabled = true;
            addBtn.IsEnabled = true;
            editBtn.IsEnabled = true;
            medEditButton.IsEnabled = true;
            saveBtn.IsEnabled = true;
            updateBtn.IsEnabled = true;
            medUpdateButton.IsEnabled = true;
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

                string validate = "SELECT COUNT(*) FROM patients WHERE pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year";

                SqlCommand cmd = new SqlCommand(validate, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

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

                        query = "select pid from patients where pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year";
                        cmd.CommandText = query;

                        Int32 pid = -1;
                        try
                        {
                            sqlConnection.connection.Open();
                            pid = (Int32)cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            sqlConnection.connection.Close();
                        }
                        if (pid != -1)
                        {


                            foreach (Meds m in med)
                            {
                                SqlCommand cmnd = new SqlCommand();
                                cmnd.Connection = sqlConnection.connection;

                                string insert = "INSERT INTO medicine (phID, pid, wdNo, genName, enabled, dosage, frequency, period, date) VALUES (@phID, @pid, @wdNo, @genName, @enabled, @dosage, @frequency, @period, @date);";

                                cmnd.CommandText = insert;

                                cmnd.Parameters.AddWithValue("@phID", Convert.ToInt32(m.genName.ToString().Split().Last().Trim()));
                                cmnd.Parameters.AddWithValue("@pid", pid);
                                cmnd.Parameters.AddWithValue("@wdNo", int.Parse(this.wdNo.Text));
                                cmnd.Parameters.AddWithValue("@genName", m.genName.ToString().Split().First().Trim());
                                cmnd.Parameters.AddWithValue("@enabled", Convert.ToInt32(m.enable));
                                cmnd.Parameters.AddWithValue("@dosage", Convert.ToInt32(m.dosage));
                                cmnd.Parameters.AddWithValue("@frequency", Convert.ToInt32(m.frequency));
                                cmnd.Parameters.AddWithValue("@period", Convert.ToInt32(m.period));
                                cmnd.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());

                                try
                                {
                                    sqlConnection.connection.Open();
                                    cmnd.ExecuteNonQuery();
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

                            showInfoDialog("Record has been Saved !");

                            query = "select * from patients where pid=@pid;";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@pid", pid);
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

                            query = "SELECT phID,  genName, enabled, dosage, frequency, period FROM medicine WHERE pid=@pid;";
                            cmd.CommandText = query;

                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.SelectCommand = cmd;

                            try
                            {
                                readDBMed(adapter);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            disableItems();
                            disableButtons();
                            cancelBtn.IsEnabled = true;

                            patients.Add(new Patient { pNicNo = this.pNicNo.Text, pBhtNo = String.Format("{0:00000}", this.pBhtNo.Text), year = int.Parse(DateTime.Now.Year.ToString()), wdNo = int.Parse(wdNo.Text.ToString()), pEnableCheck = enable, pName = (pMiddleName.Text == "") ? this.pFirstName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper() : this.pFirstName.Text.ToUpper() + " " + this.pMiddleName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper() });
                        }
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

                        MessageDialogResult result = await this.ShowMessageAsync("Info Message!", "Record for the current patient has found !\nDo you want to Edit or View this Record ?",
                            MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                        if (result == MessageDialogResult.Affirmative)
                        {

                            string query = "SELECT pid FROM patients WHERE pNicNo=@pNicNo and pBhtNo=@pBhtNo and year=@year;";


                            cmd.CommandText = query;
                            cmd.Connection = sqlConnection.connection;
                            cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                            cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                            cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

                            Int32 pid = -1;
                            try
                            {
                                sqlConnection.connection.Open();
                                pid = (Int32)cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                sqlConnection.connection.Close();
                            }

                            if (pid != -1)
                            {
                                query = "select * from patients where pid=@pid;";
                                cmd.CommandText = query;

                                cmd.Parameters.AddWithValue("@pid", pid);

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

                                query = "SELECT phID, genName, enabled, dosage, frequency, period FROM medicine WHERE pid=@pid;";
                                cmd.CommandText = query;

                                SqlDataAdapter adapter = new SqlDataAdapter();
                                adapter.SelectCommand = cmd;

                                try
                                {
                                    readDBMed(adapter);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                disableButtons();
                                disableItems();
                                editBtn.IsEnabled = true;
                                deleteBtn.IsEnabled = true;

                                patientsDataGrid.SelectedItem = patients.Where(x => x.pNicNo == this.pNicNo.Text && x.pBhtNo == String.Format("{0:00000}", this.pBhtNo.Text) && x.year == int.Parse(DateTime.Now.Year.ToString())).FirstOrDefault();
                                patientsDataGrid.IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            clearItems();
            disableItems();
            patientsDataGrid.IsEnabled = true;
            patientsDataGrid.SelectedIndex = -1;
            disableButtons();
            med.Clear();

            wdNo.SelectedIndex = user - 1;
            addBtn.IsEnabled = true;
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
                medUpdateButton.IsEnabled = true;
                enableItems();
                patientsDataGrid.IsEnabled = false;
            }
        }

        private void readDB(SqlDataReader reader)
        {
            reader.Read();

            wdNo.SelectedIndex = int.Parse(reader["wdNo"].ToString().Trim()) - 1;

            pBhtNo.Text = String.Format("{0:00000}", Convert.ToInt32(reader["pBhtNo"]));

            bool enab = Convert.ToInt32(reader["pEnableCheck"]) == 0 ? false : true;
            pEnableCheck.IsChecked = enab;

            DateTime regDate = Convert.ToDateTime(reader["pRegDate"]);
            pRegDate.Text = regDate.ToString("dd : MMM : yyyy");

            pFirstName.Text = reader["pFirstName"].ToString().Trim();
            pMiddleName.Text = reader["pMiddleName"].ToString().Trim();
            pLastName.Text = reader["pLastName"].ToString().Trim();

            if (user <= 12 || user >= 20)
            {
                pNicNo.Text = reader["pNicNo"].ToString().Trim();

                pDob.SelectedDate = DateTime.Parse(reader["pDob"].ToString().Trim());

                pContactNo.Text = reader["pContactNo"].ToString().Trim();
                pStatus.SelectedValue = reader["pStatus"].ToString().Trim();

                pAddr.Text = reader["pAddr"].ToString().Trim();
                gName.Text = reader["gName"].ToString().Trim();
                gRelation.SelectedValue = reader["gRelation"].ToString().Trim();
                gContactNo.Text = reader["gContactNo"].ToString().Trim();
            }

            pHistory.Text = reader["pHistory"].ToString().Trim();
            pPrescription.Text = reader["pPrescription"].ToString().Trim();

        }

        private void readDBMed(SqlDataAdapter adapter)
        {
            med.Clear();

            DataSet medicineDataSet = new DataSet();

            adapter.Fill(medicineDataSet);

            foreach (DataRow row in medicineDataSet.Tables[0].Rows)
            {
                Meds m = new Meds()
                {
                    phID = Convert.ToInt32(row[0].ToString().Trim()),
                    genName = row[1].ToString().Trim(),
                    enable = Convert.ToInt32(row[2]),
                    dosage = Convert.ToInt32(row[3]),
                    frequency = Convert.ToInt32(row[4]),
                    period = Convert.ToInt32(row[5])
                };
                med.Add(m);
            }
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
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection.connection;

                if (editBtn.IsEnabled == false || this.pNicNo.Text != "")
                {
                    query = "SELECT pid FROM patients WHERE pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year";

                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
                    cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(pBhtNo.Text));
                    cmd.Parameters.AddWithValue("@year", int.Parse(DateTime.Now.Year.ToString()));

                }
                else if (editBtn.IsEnabled == true)
                {
                    Patient ePatient = patientsDataGrid.SelectedItem as Patient;
                    string nic = ePatient.pNicNo;
                    string bht = ePatient.pBhtNo;

                    int year = ePatient.year;

                    query = "SELECT pid FROM patients WHERE pNicNo=@pNicNo and pBhtNo=@pBhtNo and year=@year;";

                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@pNicNo", nic);
                    cmd.Parameters.AddWithValue("@pBhtNo", Convert.ToInt32(bht));
                    cmd.Parameters.AddWithValue("@year", year);
                }

                Int32 pid = -1;
                try
                {
                    sqlConnection.connection.Open();
                    pid = (Int32)cmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                if (pid != -1)
                {

                    query = "DELETE FROM patients WHERE pid=@pid; DELETE FROM medicine WHERE pid=@pid;";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@pid", pid);

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

                    patients.Remove(patientsDataGrid.SelectedItem as Patient);
                    showInfoDialog("Record has been deleted");

                    cancelBtn_Click(sender, e);
                    patientsDataGrid.IsEnabled = true;
                }
            }
        }

        private void normHelp_Click(object sender, RoutedEventArgs e)
        {
            if (helpFly.IsOpen == false)
            {
                helpFly.IsOpen = true;
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            if (aw == null)
            {
                aw = new AboutWindow();
                aw.Owner = this;
                aw.Closed += aw_Closed;
                aw.Owner = this;
                aw.Show();
            }
            else
            {
                aw.Activate();
            }

        }

        void aw_Closed(object sender, EventArgs e)
        {
            aw = null;
            //throw new NotImplementedException();
        }

        private void pharmaceuticals_Click(object sender, RoutedEventArgs e)
        {
            if (pw == null)
            {
                pw = new PharmaceuticalWindow(user);
                pw.Owner = this;
                pw.Closed += pw_Closed;
                pw.Show();
            }
            else
            {
                pw.Activate();
            }
        }

        void pw_Closed(object sender, EventArgs e)
        {
            pw = null;
            //throw new NotImplementedException();
        }

        private void users_Click(object sender, RoutedEventArgs e)
        {
            if (uw == null)
            {
                uw = new UserWindow();
                uw.Owner = this;
                uw.Closed += uw_Closed;
                uw.Show();
            }
            else
            {
                uw.Activate();
            }
        }

        void uw_Closed(object sender, EventArgs e)
        {
            uw = null;
            //throw new NotImplementedException();
        }

        private void colorChange_Click(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(((Button)sender).Content.ToString()), theme.Item1);
        }

        private async void updateBtn_Click(object sender, RoutedEventArgs e)
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
                Patient ePatient = patientsDataGrid.SelectedItem as Patient;

                string query = "select pID from patients where pNicNo= @epNicNo and pBhtNo= @epBhtNo and year= @eyear";
                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@epNicNo", ePatient.pNicNo);
                cmd.Parameters.AddWithValue("@epBhtNo", ePatient.pBhtNo);
                cmd.Parameters.AddWithValue("@eyear", ePatient.year);

                Int32 pid = -1;

                try
                {
                    sqlConnection.connection.Open();
                    pid = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                if (pid != -1)
                {
                    int enable = (this.pEnableCheck.IsChecked == true) ? 1 : 0;
                    query = "update patients set pNicNo= @pNicNo, pBhtNo= @pBhtNo, year= @year, wdNo= @wdNo, pEnableCheck= @pEnableCheck,pRegDate= @pRegDate, pFirstName= @pFirstName, pMiddleName= @pMiddleName, pLastName= @pLastName, pDob= @pDob, pContactNo= @pContactNo, pStatus= @pStatus, pAddr= @pAddr, gName= @gName, gRelation= @gRelation, gContactNo= @gContactNo, pHistory= @pHistory, pPrescription= @pPrescription where pID=@pID";

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@pID", pid);
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

                    patients.Remove(patientsDataGrid.SelectedItem as Patient);

                    showInfoDialog("Record has been Updated !");

                    string pName = null;

                    if (this.pMiddleName.Text == "")
                    {
                        pName = this.pFirstName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                    }
                    else
                    {
                        pName = this.pFirstName.Text.ToUpper() + " " + this.pMiddleName.Text.ToUpper() + " " + this.pLastName.Text.ToUpper();
                    }

                    patients.Add(new Patient { pNicNo = this.pNicNo.Text, pBhtNo = String.Format("{0:00000}", this.pBhtNo.Text), year = int.Parse(DateTime.Now.Year.ToString()), wdNo = int.Parse(wdNo.Text.ToString()), pEnableCheck = enable, pName = pName });

                    patientsDataGrid.SelectedItem = patients.Where(x => x.pNicNo == this.pNicNo.Text && x.pBhtNo == String.Format("{0:00000}", this.pBhtNo.Text) && x.year == int.Parse(DateTime.Now.Year.ToString())).FirstOrDefault();
                    patientsDataGrid.IsEnabled = true;
                    addBtn.IsEnabled = true;
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
         */

        private void sourceCollectionUpdate()
        {

            string query = "SELECT pNicNo, pBhtNo, year, wdNo, pEnableCheck, pFirstName, pMiddleName, pLastName FROM patients";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
            DataSet patientDataSet = new DataSet();
            try
            {
                adapter.Fill(patientDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataRow row in patientDataSet.Tables[0].Rows)
            {
                Patient patient = new Patient()
                {
                    pNicNo = row[0].ToString().Trim(),
                    pBhtNo = String.Format("{0:00000}", row[1]),
                    year = Convert.ToInt32(row[2]),
                    wdNo = Convert.ToInt32(row[3]),
                    pEnableCheck = Convert.ToInt32(row[4]),
                    pName = (row[6] != System.DBNull.Value) ? row[5].ToString().Trim() + " " + row[6].ToString().Trim() + " " + row[7].ToString().Trim() : row[5].ToString().Trim() + " " + row[7].ToString().Trim()
                };
                patients.Add(patient);
            }
        }

        private void patientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (patientsDataGrid.SelectedIndex != -1)
            {
                Patient ePatient = patientsDataGrid.SelectedItem as Patient;
                
                string nic = ePatient.pNicNo;
                string bht = ePatient.pBhtNo;
                int year = ePatient.year;

                string query = "SELECT pid FROM patients WHERE pNicNo=@nic and pBhtNo=@pBhtNo and year=@year;";

                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);

                cmd.Parameters.AddWithValue("@nic", nic);
                cmd.Parameters.AddWithValue("@pBhtNo", bht);
                cmd.Parameters.AddWithValue("@year", year);

                Int32 pid = -1;

                try
                {
                    sqlConnection.connection.Open();
                    pid = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                query = "SELECT * FROM patients WHERE pid=@pid;";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@pid", pid);

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

                query = "SELECT phID, genName, enabled, dosage, frequency, period FROM medicine WHERE pid=@pid;";
                cmd.CommandText = query;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;

                try
                {
                    readDBMed(adapter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                if (user > 12 && user < 20)
                {
                    addBtn.IsEnabled = false;
                    editBtn.IsEnabled = false;
                    updateBtn.IsEnabled = false;
                    deleteBtn.IsEnabled = false;

                    medEditButton.IsEnabled = false;
                    medUpdateButton.IsEnabled = false;

                    medicineDataGrid.IsReadOnly = true;
                    medicineDataGrid.IsEnabled = true;
                }
                else
                {
                    if (ePatient.wdNo != user)
                    {
                        editBtn.IsEnabled = false;
                        deleteBtn.IsEnabled = false;
                        medEditButton.IsEnabled = false;
                    }
                    else
                    {
                        editBtn.IsEnabled = true;
                        deleteBtn.IsEnabled = true;
                        medEditButton.IsEnabled = true;
                    }
                    wdNo.IsEnabled = false;
                    addBtn.IsEnabled = false;
                    
                }

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

        private void stock_Click(object sender, RoutedEventArgs e)
        {
            if (sw == null)
            {
                sw = new StockWindow(user, userID);
                sw.Owner = this;
                sw.Closed += sw_Closed;
                sw.Owner = this;
                sw.Show();
            }
            else
            {
                sw.Activate();
            }

        }

        private void sw_Closed(object sender, EventArgs e)
        {
            sw = null;
            //throw new NotImplementedException();
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

        private void medEditButton_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            medUpdateButton.IsEnabled = true;
            cancelBtn.IsEnabled = true;
            medicineDataGrid.IsEnabled = true;
        }

        private void medUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection.connection;

            string y = this.pRegDate.Text.Replace(" : ", "/");
            int year = Convert.ToInt32(Convert.ToDateTime(y).ToString("yyyy"));

            string query = "SELECT pid FROM patients WHERE pNicNo= @pNicNo and pBhtNo= @pBhtNo and year= @year;";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@pNicNo", this.pNicNo.Text);
            cmd.Parameters.AddWithValue("@pBhtNo", int.Parse(this.pBhtNo.Text));
            cmd.Parameters.AddWithValue("@year", year);

            Int32 pid = -1;

            try
            {
                sqlConnection.connection.Open();
                pid = (Int32)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.connection.Close();
            }

            if (pid != -1)
            {
                foreach (Meds m in med)
                {
                    SqlCommand cmnd = new SqlCommand();
                    cmnd.Connection = sqlConnection.connection;

                    if (m.phID == 0)
                    {
                        string insert = "INSERT INTO medicine (phID, pid, wdNo, genName, enabled, dosage, frequency, period, date) VALUES (@phID, @pid, @wdNo, @genName, @enabled, @dosage, @frequency, @period, @date);";

                        cmnd.CommandText = insert;
                        cmnd.Parameters.AddWithValue("@phID", Convert.ToInt32(m.genName.ToString().Split().Last().Trim()));
                        cmnd.Parameters.AddWithValue("@pid", pid);
                        cmnd.Parameters.AddWithValue("@wdNo", int.Parse(this.wdNo.Text));
                        cmnd.Parameters.AddWithValue("@genName", m.genName.ToString().Split().First().Trim());
                        cmnd.Parameters.AddWithValue("@enabled", Convert.ToInt32(m.enable));
                        cmnd.Parameters.AddWithValue("@dosage", Convert.ToInt32(m.dosage));
                        cmnd.Parameters.AddWithValue("@frequency", Convert.ToInt32(m.frequency));
                        cmnd.Parameters.AddWithValue("@period", Convert.ToInt32(m.period));
                        cmnd.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());

                        try
                        {
                            sqlConnection.connection.Open();
                            cmnd.ExecuteNonQuery();
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
                    else
                    {

                        query = "SELECT COUNT(*) FROM medicine WHERE phID=@phID AND pid=@pid AND dosage=@dosage AND frequency=@frequency AND period= @period;";

                        cmnd.CommandText = query;
                        cmnd.Parameters.AddWithValue("@phID", m.phID);
                        cmnd.Parameters.AddWithValue("@pid", pid);
                        cmnd.Parameters.AddWithValue("@dosage", m.dosage);
                        cmnd.Parameters.AddWithValue("@frequency", m.frequency);
                        cmnd.Parameters.AddWithValue("@period", m.period);

                        Int32 count = -1;

                        try
                        {
                            sqlConnection.connection.Open();
                            count = (Int32)cmnd.ExecuteScalar();
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


                            if (count != 0)
                            {
                                query = "UPDATE medicine SET enabled=@enabled WHERE pid=@pid AND phID=@phID;";
                                cmnd.CommandText = query;
                            }
                            else
                            {
                                query = "UPDATE medicine SET enabled=@enabled, dosage=@dosage, frequency=@frequency, period=@period, date=@date WHERE pid=@pid AND phID=@phID;";
                                cmnd.CommandText = query;
                                cmnd.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                            }

                            cmnd.Parameters.AddWithValue("@enabled", Convert.ToInt32(m.enable));

                            try
                            {
                                sqlConnection.connection.Open();
                                cmnd.ExecuteNonQuery();
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
                    medEditButton.IsEnabled = true;
                    medUpdateButton.IsEnabled = false;

                    medicineDataGrid.IsEnabled = false;
                }
            }
        }

        private void disableRequestItems()
        {
            requestDataGrid.IsEnabled = false;
            requestID.IsEnabled = false;
        }

        private void enableRequestItems()
        {
            requestDataGrid.IsEnabled = true;
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
            requestDataGrid.IsEnabled = true;

            if (user > 0 && user <= 12)
            {
                requestID.Text = "WD" + (user.ToString()).PadLeft(2, '0') + " " + DateTime.Now.ToString("dd MM yy");
            }
            else if (user == 13)
            {
                requestID.Text = "IPD" + " " + DateTime.Now.ToString("dd MM yy");
            }
            else if (user == 14)
            {
                requestID.Text = "OPD" + " " + DateTime.Now.ToString("dd MM yy");
            }

            reqGenButton.IsEnabled = true;
            reqSubButton.IsEnabled = true;
            reqCancelButton.IsEnabled = true;
        }

        private void reqGenButton_Click(object sender, RoutedEventArgs e)
        {
            request.Clear();

            if (user <= 12)
            {
                string query = "SELECT medicine.phID,  pharmaceuticals.genName, pharmaceuticals.type, medicine.dosage, SUM(medicine.frequency * DATEDIFF(day,  @today, medicine.date)) AS quantity FROM medicine FULL OUTER JOIN pharmaceuticals ON pharmaceuticals.phID=medicine.phID WHERE wdNo=@wdNo AND enabled=1 GROUP BY medicine.phID, medicine.dosage, pharmaceuticals.genName, pharmaceuticals.type ORDER BY phID;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection.connection;
                cmd.CommandText = query;

                Console.WriteLine(DateTime.Now.AddDays(14).ToShortDateString());

                cmd.Parameters.AddWithValue("@today", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@wdNo", user);

                DataSet requestDataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;

                try
                {
                    adapter.Fill(requestDataSet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                foreach (DataRow row in requestDataSet.Tables[0].Rows)
                {
                    Medicine i = new Medicine()
                    {
                        phID = Convert.ToInt32(row[0].ToString().Trim()),
                        genName = row[1].ToString().Trim(),
                        type = row[2].ToString().Trim(),
                        dosage = Convert.ToInt32(row[3].ToString().Trim()),
                        quantity = Convert.ToInt32(row[4].ToString().Trim()),
                    };
                    request.Add(i);
                }
            }
            else if (user == 13)
            {
                string query = "SELECT reqFromIPD.phID,  pharmaceuticals.genName, pharmaceuticals.type, reqFromIPD.dosage, SUM(quantity) AS quantity FROM reqFromIPD FULL OUTER JOIN pharmaceuticals ON pharmaceuticals.phID=reqFromIPD.phID WHERE reqFromIPD.phID IS NOT NULL GROUP BY reqFromIPD.phID, reqFromIPD.dosage, pharmaceuticals.genName, pharmaceuticals.type;";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection.connection;
                cmd.CommandText = query;

                DataSet requestDataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;

                try
                {
                    adapter.Fill(requestDataSet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                foreach (DataRow row in requestDataSet.Tables[0].Rows)
                {
                    Medicine i = new Medicine()
                    {
                        phID = Convert.ToInt32(row[0].ToString().Trim()),
                        genName = row[1].ToString().Trim(),
                        type = row[2].ToString().Trim(),
                        dosage = Convert.ToInt32(row[3].ToString().Trim()),
                        quantity = Convert.ToInt32(row[4].ToString().Trim()),
                    };
                    request.Add(i);
                }

            }
            else if (user == 14)
            {

            }

            reqCancelButton.IsEnabled = true;
        }

        private async void reqSubButton_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                FirstAuxiliaryButtonText = "Cancel",
                ColorScheme = MetroDialogOptions.ColorScheme
            };

            MessageDialogResult result = await this.ShowMessageAsync("Info Message!", "Are you sure do you want to request the medicines ?",
            MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                string reqID = requestID.Text;

                foreach (Medicine m in request)
                {
                    Console.WriteLine(m.genName + " " + m.dosage + " " + m.quantity);
                }

                foreach (Medicine i in request)
                {
                    string query = "";

                    if (user < 13)
                    {
                        query = "INSERT INTO reqFromIPD (reqID, phID, dosage, quantity, enable) VALUES (@reqID, @phID, @dosage, @quantity, 1);";
                    }
                    else if (user == 13)
                    {
                        query = "INSERT INTO reqFromStore (reqID, phID, dosage, quantity, enable) VALUES (@reqID, @phID, @dosage, @quantity, 1);";
                    }


                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection.connection;
                    cmd.CommandText = query;

                    if (i.phID == 0)
                    {
                        cmd.Parameters.AddWithValue("@phID", Convert.ToInt32(i.genName.Split().Last()));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@phID", Convert.ToInt32(i.phID));
                    }

                    cmd.Parameters.AddWithValue("@reqID", reqID);
                    cmd.Parameters.AddWithValue("@dosage", Convert.ToInt32(i.dosage));
                    cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(i.quantity));

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
                requestID.Text = "";
                request.Clear();
                requestDataGrid.IsEnabled = false;
                reqGenButton.IsEnabled = false;
                reqAddButton.IsEnabled = true;
                reqCancelButton.IsEnabled = false;
                reqSubButton.IsEnabled = false;
            }
        }

        private void confirmNotifier()
        {
            if (user <= 13)
            {
                string query = null;

                switch (user)
                {
                    case 1:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD01%';";
                        break;
                    case 2:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD02%';";
                        break;
                    case 3:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD03%';";
                        break;
                    case 4:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD04%';";
                        break;
                    case 5:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD05%';";
                        break;
                    case 6:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD06%';";
                        break;
                    case 7:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD07%';";
                        break;
                    case 8:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD08%';";
                        break;
                    case 9:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD09%';";
                        break;
                    case 10:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD10%';";
                        break;
                    case 11:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD11%';";
                        break;
                    case 12:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmIPD] WHERE reqID LIKE 'WD12%';";
                        break;
                    case 13:
                        query = "SELECT DISTINCT reqID FROM [GP].[dbo].[confirmStore];";
                        break;
                }

                SqlDependency.Stop(sqlConnection.conString);
                SqlDependency.Start(sqlConnection.conString);

                foreach (Request r in con)
                {
                    requestList.Remove(r);
                }

                con.Clear();

                using (SqlCommand cmd = sqlConnection.connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Notification = null;

                    SqlDependency conf = new SqlDependency(cmd);
                    conf.OnChange += new OnChangeEventHandler(conf_OnChange);

                    DataSet notifReqDataSet = new DataSet();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(notifReqDataSet);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    foreach (DataRow row in notifReqDataSet.Tables[0].Rows)
                    {
                        Request r = new Request()
                        {
                            request = row[0].ToString()
                        };
                        con.Add(r);
                    }
                }
                foreach (Request r in con)
                {
                    requestList.Add(r);
                }


            }
        }

        void conf_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void requestNotifier()
        {

            if (user == 13 || user == 15)
            {
                string query = null;

                if (user == 13)
                {
                    query = "SELECT DISTINCT reqID FROM [GP].[dbo].[reqFromIPD] WHERE enable=1;";
                }
                else if (user == 15)
                {
                    query = "SELECT DISTINCT reqID FROM [GP].[dbo].[reqFromStore] WHERE enable=1;";
                }

                SqlDependency.Stop(sqlConnection.conString);
                SqlDependency.Start(sqlConnection.conString);


                foreach (Request r in req)
                {
                    requestList.Remove(r);
                }

                req.Clear();


                using (SqlCommand cmd = sqlConnection.connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Notification = null;

                    SqlDependency dep = new SqlDependency(cmd);
                    //dep.OnChange +=dep_OnChange;
                    dep.OnChange += new OnChangeEventHandler(dep_OnChange);

                    DataSet notifReqDataSet = new DataSet();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(notifReqDataSet);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    foreach (DataRow row in notifReqDataSet.Tables[0].Rows)
                    {
                        Request r = new Request()
                        {
                            request = row[0].ToString()
                        };
                        req.Add(r);
                    }
                }
                foreach (Request r in req)
                {
                    requestList.Add(r);
                }

            }

        }

        private void dep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            /*
            if (this.Dispatcher.Thread != Thread.CurrentThread)
            {
                notifReqDataGrid.Dispatcher.BeginInvoke(new Action(requestNotifier));
            }
            else
            {
                requestNotifier();
            }

            SqlDependency dep = sender as SqlDependency;

            dep.OnChange -= new OnChangeEventHandler(dep_OnChange);
            */
            //throw new NotImplementedException();
        }

        private void notifReqDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (notifReqDataGrid.SelectedIndex != -1)
            {
                acceptButton.IsEnabled = false;
                confirmButton.IsEnabled = false;
                rejectButton.IsEnabled = true;
                clearButton.IsEnabled = true;

                reqDataNotifyDataGrid.ItemsSource = notify;

                Request r = notifReqDataGrid.SelectedItem as Request;

                if (r.request.Trim().EndsWith(" CON"))
                {
                    acceptButton.IsEnabled = true;
                    string query = "";
                    switch (user)
                    {
                        case 1:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd01.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd01.phID AND confirmIPD.dosage=wd01.dosage AND confirmIPD.expiryDate=wd01.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 2:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd02.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd02.phID AND confirmIPD.dosage=wd02.dosage AND confirmIPD.expiryDate=wd02.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 3:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd03.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd03.phID AND confirmIPD.dosage=wd03.dosage AND confirmIPD.expiryDate=wd03.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 4:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd04.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd04.phID AND confirmIPD.dosage=wd04.dosage AND confirmIPD.expiryDate=wd04.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 5:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd05.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd05.phID AND confirmIPD.dosage=wd05.dosage AND confirmIPD.expiryDate=wd05.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 6:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd06.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd06.phID AND confirmIPD.dosage=wd06.dosage AND confirmIPD.expiryDate=wd06.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 7:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd07.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd07.phID AND confirmIPD.dosage=wd07.dosage AND confirmIPD.expiryDate=wd07.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 8:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd08.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd08.phID AND confirmIPD.dosage=wd08.dosage AND confirmIPD.expiryDate=wd08.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 9:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd09.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd09.phID AND confirmIPD.dosage=wd09.dosage AND confirmIPD.expiryDate=wd09.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 10:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd10.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd10.phID AND confirmIPD.dosage=wd10.dosage AND confirmIPD.expiryDate=wd10.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 11:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd11.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd11.phID AND confirmIPD.dosage=wd11.dosage AND confirmIPD.expiryDate=wd11.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 12:
                            query = "SELECT confirmIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmIPD.dosage, reqFromIPD.quantity, ISNULL(wd12.quantity, 0), confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromIPD ON confirmIPD.phID=reqFromIPD.phID AND confirmIPD.dosage=reqFromIPD.dosage FULL OUTER JOIN wd01 ON confirmIPD.phID=wd12.phID AND confirmIPD.dosage=wd12.dosage AND confirmIPD.expiryDate=wd12.expiryDate WHERE confirmIPD.reqID=@reqID ORDER BY phID, confirmIPD.expiryDate;";
                            break;
                        case 13:
                            query = "SELECT confirmStore.phID, pharmaceuticals.genName, pharmaceuticals.type, confirmStore.dosage, reqFromStore.quantity, ISNULL(ipd.quantity, 0), confirmStore.quantity FROM confirmStore FULL OUTER JOIN pharmaceuticals ON confirmStore.phID=pharmaceuticals.phID FULL OUTER JOIN reqFromStore ON confirmStore.phID=reqFromStore.phID AND confirmStore.dosage=reqFromStore.dosage FULL OUTER JOIN ipd ON confirmStore.phID=ipd.phID AND confirmStore.dosage=ipd.dosage AND confirmStore.expiryDate=ipd.expiryDate WHERE confirmStore.reqID= @reqID ORDER BY phID, confirmStore.expiryDate;";
                            break;
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection.connection;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@reqID", r.request.Trim());

                    DataSet notifyDataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(notifyDataSet);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    notify.Clear();
                    foreach (DataRow row in notifyDataSet.Tables[0].Rows)
                    {
                        NotifyRequest notifReq = new NotifyRequest()
                        {
                            phID = Convert.ToInt32(row[0].ToString().Trim()),
                            genName = row[1].ToString().Trim(),
                            type = row[2].ToString().Trim(),
                            dosage = Convert.ToInt32(row[3].ToString().Trim()),
                            quantity = Convert.ToInt32(row[4].ToString().Trim()),
                            storedQuantity = Convert.ToInt32(row[5].ToString().Trim()),
                            issue = Convert.ToInt32(row[6].ToString().Trim())
                        };
                        notify.Add(notifReq);
                    }
                    reqDataNotifyDataGrid.ItemsSource = notify;
                }
                else
                {
                    //confirmButton.IsEnabled = true;
                    generateButton.IsEnabled = true;

                    string query = null;

                    if (user < 13)
                    {
                        query = "SELECT reqFromIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, reqFromIPD.dosage, reqFromIPD.quantity, ISNULL((ipd.quantity - ISNULL(ipd.reduce, 0)), 0) as available FROM reqFromIPD FULL OUTER JOIN pharmaceuticals ON reqFromIPD.phID=pharmaceuticals.phID FULL OUTER JOIN ipd ON reqFromIPD.phID=ipd.phID AND reqFromIPD.dosage=ipd.dosage WHERE reqID= @reqID AND reqFromIPD.phID IS NOT NULL ORDER BY phID, expiryDate;";
                    }
                    else if (user == 13)
                    {
                        query = "SELECT reqFromIPD.phID, pharmaceuticals.genName, pharmaceuticals.type, reqFromIPD.dosage, reqFromIPD.quantity, ISNULL((ipd.quantity - ISNULL(ipd.reduce, 0)), 0) as available FROM reqFromIPD FULL OUTER JOIN pharmaceuticals ON reqFromIPD.phID=pharmaceuticals.phID FULL OUTER JOIN ipd ON reqFromIPD.phID=ipd.phID AND reqFromIPD.dosage=ipd.dosage WHERE reqID= @reqID AND reqFromIPD.phID IS NOT NULL AND enable=1 ORDER BY phID, expiryDate;";

                    }
                    else if (user == 15)
                    {
                        query = "SELECT reqFromStore.phID, pharmaceuticals.genName, pharmaceuticals.type, reqFromStore.dosage, reqFromStore.quantity, ISNULL((store.quantity - ISNULL(store.reduce,0)), 0) as available FROM reqFromStore FULL OUTER JOIN pharmaceuticals ON reqFromStore.phID=pharmaceuticals.phID FULL OUTER JOIN store ON reqFromStore.phID=store.phID AND reqFromStore.dosage=store.dosage WHERE reqID= @reqID AND reqFromStore.phID IS NOT NULL AND enable=1 ORDER BY phID, expiryDate;";
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection.connection;
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@reqID", r.request.Trim());

                    DataSet notifyDataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    try
                    {
                        adapter.Fill(notifyDataSet);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    notify.Clear();
                    foreach (DataRow row in notifyDataSet.Tables[0].Rows)
                    {
                        NotifyRequest notifReq = new NotifyRequest()
                        {
                            phID = Convert.ToInt32(row[0].ToString().Trim()),
                            genName = row[1].ToString().Trim(),
                            type = row[2].ToString().Trim(),
                            dosage = Convert.ToInt32(row[3].ToString().Trim()),
                            quantity = Convert.ToInt32(row[4].ToString().Trim()),
                            storedQuantity = Convert.ToInt32(row[5].ToString().Trim())
                        };
                        notify.Add(notifReq);
                    }
                    reqDataNotifyDataGrid.ItemsSource = notify;
                }
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void reqCancelButton_Click(object sender, RoutedEventArgs e)
        {
            requestID.Text = "";
            request.Clear();
            requestDataGrid.IsEnabled = false;
            reqGenButton.IsEnabled = false;
            reqAddButton.IsEnabled = true;
            reqCancelButton.IsEnabled = false;
            reqSubButton.IsEnabled = false;
        }

        /*
        private void StoreView_Loaded(object sender, RoutedEventArgs e)
        {

        }
         */

        private void logs_Click(object sender, RoutedEventArgs e)
        {
            if (lw == null)
            {
                lw = new LogsWindow();
                lw.Owner = this;
                lw.Closed += lw_Closed;
                lw.Owner = this;
                lw.Show();
            }
            else
            {
                lw.Activate();
            }
        }

        private void lw_Closed(object sender, EventArgs e)
        {
            lw = null;
            //throw new NotImplementedException();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            Request r = notifReqDataGrid.SelectedItem as Request;

            if (user == 15)
            {

                foreach (NotifyRequest nr in notify)
                {
                    SqlCommand cmd = new SqlCommand();
                    Object returnValue;

                    cmd.CommandText = "confirmProcStore";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@reqID", r.request);
                    cmd.Parameters.AddWithValue("@phID", nr.phID);
                    cmd.Parameters.AddWithValue("@dosage", nr.dosage);
                    cmd.Parameters.AddWithValue("@quantity", nr.quantity);

                    cmd.Connection = sqlConnection.connection;

                    sqlConnection.connection.Open();

                    returnValue = cmd.ExecuteScalar();

                    sqlConnection.connection.Close();
                }

                string query = "UPDATE reqFromStore SET enable=0  WHERE reqID=@reqID;";

                SqlCommand cmnd = new SqlCommand();
                cmnd.Connection = sqlConnection.connection;
                cmnd.CommandText = query;
                cmnd.Parameters.AddWithValue("@reqID", r.request);

                sqlConnection.connection.Open();
                cmnd.ExecuteNonQuery();
                sqlConnection.connection.Close();

            }
            else if (user == 13)
            {
                foreach (NotifyRequest nr in notify)
                {
                    SqlCommand cmd = new SqlCommand();
                    Object returnValue;

                    cmd.CommandText = "confirmProcIPD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@reqID", r.request);
                    cmd.Parameters.AddWithValue("@phID", nr.phID);
                    cmd.Parameters.AddWithValue("@dosage", nr.dosage);
                    cmd.Parameters.AddWithValue("@quantity", nr.quantity);

                    cmd.Connection = sqlConnection.connection;

                    sqlConnection.connection.Open();

                    returnValue = cmd.ExecuteScalar();

                    sqlConnection.connection.Close();
                }

                string query = "UPDATE reqFromIPD SET enable=0  WHERE reqID=@reqID;";

                SqlCommand cmnd = new SqlCommand();
                cmnd.Connection = sqlConnection.connection;
                cmnd.CommandText = query;
                cmnd.Parameters.AddWithValue("@reqID", r.request);

                sqlConnection.connection.Open();
                cmnd.ExecuteNonQuery();
                sqlConnection.connection.Close();
            }

            showInfoDialog("Request has been approved !");

            requestList.Remove(r);
            notify.Clear();
            generateButton.IsEnabled = false;
            confirmButton.IsEnabled = false;
            rejectButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            acceptButton.IsEnabled = false;

            notifReqDataGrid.IsEnabled = true;
            reqDataNotifyDataGrid.IsEnabled = false;
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            reqDataNotifyDataGrid.ItemsSource = null;

            if (user == 13)
            {

            }
            else if (user == 15)
            {

            }

            foreach (NotifyRequest nr in notify)
            {

                if ((nr.storedQuantity - nr.quantity) > 0)
                {
                    nr.issue = nr.quantity;
                }
                else
                {
                    nr.issue = nr.storedQuantity;
                }
            }

            reqDataNotifyDataGrid.ItemsSource = notify;
            generateButton.IsEnabled = false;
            notifReqDataGrid.IsEnabled = false;
            confirmButton.IsEnabled = true;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            generateButton.IsEnabled = false;
            acceptButton.IsEnabled = false;
            confirmButton.IsEnabled = false;
            rejectButton.IsEnabled = false;
            clearButton.IsEnabled = false;

            notifReqDataGrid.SelectedIndex = -1;
            notifReqDataGrid.IsEnabled = true;

            notify.Clear();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            notifReqDataGrid.IsEnabled = false;
            Request r = notifReqDataGrid.SelectedItem as Request;

            if (user <= 13)
            {
                string query = null;

                if (user < 13)
                {
                    query = "SELECT confirmIPD.phID, pharmaceuticals.id, pharmaceuticals.genName, pharmaceuticals.type, pharmaceuticals.category, pharmaceuticals.countable, confirmIPD.dosage, confirmIPD.expiryDate, confirmIPD.quantity FROM confirmIPD FULL OUTER JOIN pharmaceuticals ON confirmIPD.phID=pharmaceuticals.id WHERE reqID=@reqID;";
                }
                else if (user == 13)
                {
                    query = "SELECT confirmStore.phID, pharmaceuticals.id, pharmaceuticals.genName, pharmaceuticals.type, pharmaceuticals.category, pharmaceuticals.countable, confirmStore.dosage, confirmStore.expiryDate, confirmStore.quantity FROM confirmStore FULL OUTER JOIN pharmaceuticals ON confirmStore.phID=pharmaceuticals.id WHERE reqID=@reqID;";
                }

                ObservableCollection<Medicine> medi = new ObservableCollection<Medicine>();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
                adapter.SelectCommand.Parameters.AddWithValue("@reqID", r.request);
                DataSet mediDataSet = new DataSet();
                try
                {
                    adapter.Fill(mediDataSet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                foreach (DataRow row in mediDataSet.Tables[0].Rows)
                {
                    Medicine m = new Medicine()
                    {
                        phID = Convert.ToInt32(row[0]),
                        id = Convert.ToInt32(row[1]),
                        genName = row[2].ToString().Trim(),
                        type = row[3].ToString().Trim(),
                        category = row[4].ToString().Trim(),
                        countable = Convert.ToInt32(row[5]),
                        dosage = Convert.ToInt32(row[6]),
                        //expiryDate = row[7].ToString().Trim(),
                        expiryDate = DateTime.Parse(row[7].ToString().Trim()).ToShortDateString(),
                        quantity = Convert.ToInt32(row[8])
                    };
                    medi.Add(m);
                }

                MongoClient client = new MongoClient();
                var server = client.GetServer();
                var db = server.GetDatabase("StockDatabase");
                var collection = db.GetCollection<Transactions>("TransactionCollection");

                Transactions trans = new Transactions();

                string req = r.request;
                req = req.Replace(" CON", "");
                trans.reqID = req;
                trans.date = DateTime.Now;
                trans.med = medi;

                collection.Save(trans);

                SqlCommand cmd = new SqlCommand();
                Object returnValue;

                switch (user)
                {
                    case 1:
                        cmd.CommandText = "addToWD01";
                        break;
                    case 2:
                        cmd.CommandText = "addToWD02";
                        break;
                    case 3:
                        cmd.CommandText = "addToWD03";
                        break;
                    case 4:
                        cmd.CommandText = "addToWD04";
                        break;
                    case 5:
                        cmd.CommandText = "addToWD05";
                        break;
                    case 6:
                        cmd.CommandText = "addToWD06";
                        break;
                    case 7:
                        cmd.CommandText = "addToWD07";
                        break;
                    case 8:
                        cmd.CommandText = "addToWD08";
                        break;
                    case 9:
                        cmd.CommandText = "addToWD09";
                        break;
                    case 10:
                        cmd.CommandText = "addToWD10";
                        break;
                    case 11:
                        cmd.CommandText = "addToWD11";
                        break;
                    case 12:
                        cmd.CommandText = "addToWD12";
                        break;
                    case 13:
                        cmd.CommandText = "addToIPD";
                        break;
                }

                string s = r.request.Trim();
                s = s.Replace(" CON", "");

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@reqID", r.request.Trim());
                cmd.Parameters.AddWithValue("@res", s);

                cmd.Connection = sqlConnection.connection;

                try
                {
                    sqlConnection.connection.Open();
                    returnValue = cmd.ExecuteScalar();
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

            showInfoDialog("Stock accepted !");


            requestList.Remove(r);
            notify.Clear();
            generateButton.IsEnabled = false;
            confirmButton.IsEnabled = false;
            rejectButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            acceptButton.IsEnabled = false;

            notifReqDataGrid.IsEnabled = true;
            reqDataNotifyDataGrid.IsEnabled = false;
        }

        private async void rejectButton_Click(object sender, RoutedEventArgs e)
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
                notifReqDataGrid.IsEnabled = false;
                Request r = notifReqDataGrid.SelectedItem as Request;

                if (user == 13 || user == 15)
                {

                    string query = null;
                    if (user == 13)
                    {
                        query = "DELET FROM reqFromIPD WHERE reqID=@reqID;";
                    }
                    else if (user == 15)
                    {
                        query = "DELET FROM reqFromStore WHERE reqID=@reqID;";
                    }
                    SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);

                    cmd.Parameters.AddWithValue("reqID", r.request);

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
        }

    }
}
