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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Data.SqlClient;
using System.Data;
using Drug_Maintenance.Items;
using System.Collections.ObjectModel;
using System.ComponentModel;



namespace Drug_Maintenance
{
    /// <summary>
    /// Interaction logic for PharmaceuticalWindow.xaml
    /// </summary>
    public partial class PharmaceuticalWindow : MetroWindow
    {
        private ObservableCollection<Pharmaceutical> pharmaceuticals = new ObservableCollection<Pharmaceutical>();
        private string required = "* required";
        int user = -1;
        public PharmaceuticalWindow(int u)
        {
            user = u;
            InitializeComponent();

            if (user != 15)
            {
                disableButtons();
            }


            pharmaceuticalsDataGrid.ItemsSource = pharmaceuticals;
            sourceCollectionUpdate();
        }
        private int checker()
        {
            RegexUtilities util = new RegexUtilities();
            int check = 0;

            if (String.IsNullOrEmpty(id.Text))
            {
                check = 1;
                idAlert.Content = required;
            }
            else if (!util.IsValidNumber(id.Text))
            {
                check = 1;
                idAlert.Content = "Invalid ID";
            }

            if (String.IsNullOrEmpty(type.Text))
            {
                check = 1;
                typeAlert.Content = required;
            }

            if (String.IsNullOrEmpty(category.Text))
            {
                check = 1;
                categoryAlert.Content = required;
            }

            if (String.IsNullOrEmpty(genName.Text))
            {
                check = 1;
                genNameAlert.Content = required;
            }
            else if (!util.IsValidName(genName.Text))
            {
                check = 1;
                genNameAlert.Content = "Invalid Genetic Name";
            }


            return check;
        }
        private void clearFlags()
        {
            idAlert.Content = "";
            typeAlert.Content = "";
            categoryAlert.Content = "";
            genNameAlert.Content = "";
        }
        private void clearItems()
        {
            this.id.Text = "";
            this.type.SelectedIndex = -1;
            this.type.Text = "";
            this.countable.IsChecked = false;
            this.category.SelectedIndex = -1;
            this.category.Text = "";
            this.genName.Text = "";
        }
        private void enableItems()
        {
            id.IsEnabled = true;
            type.IsEnabled = true;
            countable.IsEnabled = true;
            category.IsEnabled = true;
            genName.IsEnabled = true;
        }
        private void disableItems()
        {
            this.id.IsEnabled = false;
            this.type.IsEnabled = false;
            this.countable.IsEnabled = false;
            this.category.IsEnabled = false;
            this.genName.IsEnabled = false;
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
        private void readDB(SqlDataReader reader)
        {
            reader.Read();

            this.id.Text = String.Format("{0:000}", Convert.ToInt32(reader["id"]));

            bool count = Convert.ToInt32(reader["countable"]) == 0 ? false : true;
            this.countable.IsChecked = count;

            this.type.SelectedValue = reader["type"].ToString().Trim();
            this.category.SelectedValue = reader["category"].ToString().Trim();
            this.genName.Text = reader["genName"].ToString().Trim();
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            disableButtons();
            saveBtn.IsEnabled = true;
            clearFlags();
            clearItems();
            enableItems();

            cancelBtn.IsEnabled = true;
            pharmaceuticalsDataGrid.SelectedIndex = -1;
            pharmaceuticalsDataGrid.IsEnabled = false;
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearFlags();
            clearItems();
            disableItems();
            disableButtons();
            addBtn.IsEnabled = true;
            //pharmaceuticals.Clear();
            pharmaceuticalsDataGrid.SelectedIndex = -1;
            pharmaceuticalsDataGrid.IsEnabled = true;
        }
        private void sourceCollectionUpdate()
        {

            string query = "SELECT id, type, category, countable, genName FROM pharmaceuticals";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(query, sqlConnection.connection);
            DataSet pharmaceuticalDataSet = new DataSet();

            try
            {
                adapter.Fill(pharmaceuticalDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (DataRow row in pharmaceuticalDataSet.Tables[0].Rows)
            {
                Pharmaceutical pharmaceutical = new Pharmaceutical()
                {
                    id = String.Format("{0:000}", row["id"]),
                    type = row["type"].ToString().Trim(),
                    category = row["category"].ToString().Trim(),
                    countable = Convert.ToInt32(row["countable"]),
                    genName = row["genName"].ToString().Trim()
                };
                pharmaceuticals.Add(pharmaceutical);
            }
        }
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pharmaceuticalsDataGrid.SelectedIndex == -1)
            {
                enableItems();
            }
            else
            {
                pharmaceuticalsDataGrid.IsEnabled = false;
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
                string validate = "select count(*) from pharmaceuticals where id= @id and type= @type ";

                SqlCommand cmd = new SqlCommand(validate, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@id", int.Parse(this.id.Text));
                cmd.Parameters.AddWithValue("@type", this.type.Text);

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

                if (count == 0)
                {
                    int cou = (this.countable.IsChecked == true) ? 1 : 0;
                    string query = "insert into pharmaceuticals (id, type, category, countable, genName) values( @id, @type, @category, @countable, @genName) ";

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@category", this.category.Text);
                    cmd.Parameters.AddWithValue("@countable", cou);
                    cmd.Parameters.AddWithValue("@genName", this.genName.Text.ToUpper());

                    cmd.Connection = sqlConnection.connection;

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

                    query = "select * from pharmaceuticals where id= @id and type= @type ";
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

                    pharmaceuticals.Add(new Pharmaceutical { id = String.Format("{0:000}", this.id.Text), type = this.type.Text, countable = cou, category = this.category.Text, genName = this.genName.Text.ToUpper() });

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

                    MessageDialogResult result = await this.ShowMessageAsync("Info Message!", "Record for the current pharmaceutical has found !\nDo you want to edit this Record ?",
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                    if (result == MessageDialogResult.Affirmative)
                    {

                        string query = "select * from pharmaceuticals where id= @id and type= @type ";

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

                        editBtn.IsEnabled = true;
                        deleteBtn.IsEnabled = true;
                    }

                    pharmaceuticalsDataGrid.SelectedItem = pharmaceuticals.Where(x => x.id == String.Format("{0:0000}", this.id.Text) && x.type == this.type.Text).FirstOrDefault();

                }
                disableItems();
                cancelBtn.IsEnabled = true;
                pharmaceuticalsDataGrid.IsEnabled = true;
            }

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
                Pharmaceutical ePharmaceutical = pharmaceuticalsDataGrid.SelectedItem as Pharmaceutical;

                string query = "select phID from pharmaceuticals where id= @eid and type = @etype";
                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@eid", ePharmaceutical.id);
                cmd.Parameters.AddWithValue("@etype", ePharmaceutical.type);

                Int32 phID = -1;

                try
                {
                    sqlConnection.connection.Open();
                    phID = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection.connection.Close();
                }

                if (phID != -1)
                {

                    int cou = (this.countable.IsChecked == true) ? 1 : 0;
                    query = "update pharmaceuticals set id= @id, type= @type, category= @category, countable= @countable, genName= @genName where phID=@phID ";

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@phID", phID);
                    cmd.Parameters.AddWithValue("@id", int.Parse(this.id.Text));
                    cmd.Parameters.AddWithValue("@type", this.type.Text);
                    cmd.Parameters.AddWithValue("@countable", cou);
                    cmd.Parameters.AddWithValue("@category", this.category.Text);
                    cmd.Parameters.AddWithValue("@genName", this.genName.Text.ToUpper());

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
                    deleteBtn.IsEnabled = false;
                    disableItems();

                    pharmaceuticals.Remove(pharmaceuticalsDataGrid.SelectedItem as Pharmaceutical);
                    showInfoDialog("Record has been Updated");

                    pharmaceuticals.Add(new Pharmaceutical { id = String.Format("{0:0000}", this.id.Text), type = this.type.Text, countable = cou, category = this.category.Text, genName = this.genName.Text.ToUpper() });

                    pharmaceuticalsDataGrid.IsEnabled = true;
                    addBtn.IsEnabled = true;
                }
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
                string query = null;

                if (editBtn.IsEnabled == false || this.id.Text != "")
                {
                    query = "delete from pharmaceuticals where id= @id and type= @type";

                }
                else if (editBtn.IsEnabled == true)
                {
                    Pharmaceutical eParmaceutical = pharmaceuticalsDataGrid.SelectedItem as Pharmaceutical;

                    string pid = eParmaceutical.id;
                    string type = eParmaceutical.type;

                    query = "delete from pharmaceuticals where id=" + pid + " and type = '" + type + "' ;";
                }

                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(this.id.Text));
                cmd.Parameters.AddWithValue("@type", this.type.Text);

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

                pharmaceuticals.Remove(pharmaceuticalsDataGrid.SelectedItem as Pharmaceutical);
                showInfoDialog("Record has been deleted");

                cancelBtn_Click(sender, e);
            }

        }
        private async void showInfoDialog(string msg)
        {
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var controller = await this.ShowProgressAsync("Info Message", "\n\n" + msg);
            await Task.Delay(2750);
            await controller.CloseAsync();
        }
        private void pharmaceuticalsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pharmaceuticalsDataGrid.SelectedIndex != -1)
            {
                addBtn.IsEnabled = false;

                Pharmaceutical ePharmaceutical = pharmaceuticalsDataGrid.SelectedItem as Pharmaceutical;

                string id = ePharmaceutical.id;
                string type = ePharmaceutical.type;

                string query = "select * from pharmaceuticals where id= @id and type= @type ";
                SqlCommand cmd = new SqlCommand(query, sqlConnection.connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@type", type);


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
                if (user != 15)
                {
                    disableButtons();
                }
                else
                {
                    editBtn.IsEnabled = true;
                    updateBtn.IsEnabled = false;
                    deleteBtn.IsEnabled = true;
                    cancelBtn.IsEnabled = true;
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
                ICollectionView cv = CollectionViewSource.GetDefaultView(pharmaceuticalsDataGrid.ItemsSource);
                if (filter == "")
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        Pharmaceutical p = o as Pharmaceutical;
                        if (t.Name == "idBox")
                        {
                            return (p.id.EndsWith(filter));
                        }
                        return (p.genName.ToUpper().StartsWith(filter.ToUpper()));
                    };
                }
            }
            catch (FormatException) { }

        }
        private void filterBySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                cancelBtn.IsEnabled = true;
                ComboBox t = (ComboBox)sender;

                string filter = (e.AddedItems[0] as ComboBoxItem).Content as string;
                ICollectionView cv = CollectionViewSource.GetDefaultView(pharmaceuticalsDataGrid.ItemsSource);
                if (filter != "")
                {
                    cv.Filter = o =>
                    {
                        Pharmaceutical p = o as Pharmaceutical;
                        if (t.Name == "typeCombo")
                        {
                            return (p.type.ToUpper().Equals(filter.ToUpper()));
                        }
                        return (p.category.ToUpper().Equals(filter.ToUpper()));
                    };
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
        }
        private void filterByCheckBoxChecking(object sender, RoutedEventArgs e)
        {
            cancelBtn.IsEnabled = true;
            bool flag = (sender as CheckBox).IsChecked.Value;
            int filter = (flag == true) ? 1 : 0;
            ICollectionView cv = CollectionViewSource.GetDefaultView(pharmaceuticalsDataGrid.ItemsSource);

            cv.Filter = o =>
            {
                Pharmaceutical p = o as Pharmaceutical;
                return (p.countable == Convert.ToInt32(filter));
            };
        }
    }
}
