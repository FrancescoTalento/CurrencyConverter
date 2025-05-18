using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace currency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        

        //Create an object for SqlCommand
        SqlCommand cmd = new SqlCommand();

        //Create object for SqlDataAdapter
        SqlDataAdapter da = new SqlDataAdapter();

        //Declare CurrencyId with int data type and assign value as 0.
        private string stringConnection = "Data Source=ALIENPC;Initial Catalog=CurrencyConverter;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
        private int CurrencyId = 0;//Declare FromAmount with double data type and assign value 0.
        private double FromAmount = 0;//Declare ToAmount with double data type and assign value 0.
        private double ToAmount = 0;

        SqlConnection con = new SqlConnection();
        public MainWindow()
        {
            InitializeComponent();
            lblCurrency.Content = "Currency Converter";
            BindCurrency();
            GetData();

            //String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public void mycon()
        {
            //Database connection string
            String Conn = "Data Source=ALIENPC;Initial Catalog=CurrencyConverter;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            con = new SqlConnection(Conn);

            //Open the connection
            con.Open();
        }
        private void BindCurrency()
        {
            mycon();

            //Create Object for DataTable
            DataTable dt = new DataTable();

            //Write SQL Query for Get Data from Database Table.
            cmd = new SqlCommand("select Id, CurrencyName from Currency_Master", con);

            //CommandType Define Which type of Command we Use for Write a Query
            cmd.CommandType = CommandType.Text;

            //It accepts a parameter that contains the command text of the object's SelectCommand property.
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            //Create a DataRow object
            DataRow newRow = dt.NewRow();

            //Assign a value to Id column
            newRow["Id"] = 0;

            //Assign value to CurrencyName column
            newRow["CurrencyName"] = "--SELECT--";

            //Insert a new row in dt with a data at 0 position
            dt.Rows.InsertAt(newRow, 0);

            //dt is not null and rows count greater than 0
            if (dt != null && dt.Rows.Count > 0)
            {
                //Assign data table data to From currency Combobox using item source property.
                cmbFromCurrency.ItemsSource = dt.DefaultView;

                //Assign data table data to To currency Combobox using item source property.
                cmbToCurrency.ItemsSource = dt.DefaultView;
            }
            con.Close();

            //To display the underlying datasource for cmbFromCurrency
            cmbFromCurrency.DisplayMemberPath = "CurrencyName";


            //To use as the actual value for the items
            cmbFromCurrency.SelectedValuePath = "Id";

            //Show default item in Combobox
            cmbFromCurrency.SelectedValue = 0;

            cmbToCurrency.DisplayMemberPath = "CurrencyName";
            cmbToCurrency.SelectedValuePath = "Id";
            cmbToCurrency.SelectedValue = 0;
        }
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Declare ConvertedValue with double data type for store currency converted value
            double ConvertedValue;

            //Check amount textbox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //If amount Textbox is Null or Blank show the below message box
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //After click on OK button set focus on amount textbox
                txtCurrency.Focus();
                return;
            }
            //Else if currency From is not selected or select default text --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                //Show the message
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus to From Combobox
                cmbFromCurrency.Focus();
                return;
            }
            // Else if currency To is not selected or select default text --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                //Show the message
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //Set focus to To Combobox
                cmbToCurrency.Focus();
                return;
            }

            //If From and To Combobox selects same value
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //Amount textbox value set in ConvertedValue. double.parse is used for change datatype from string to double.
                //Textbox text have string and ConvertedValue as double datatype
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Show the label converted currency and converted currency name.
                //Tostring("N3") is used to place 000 after the dot(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //Calculation for currency converter is From currency value multiplied(*) with the amount textbox value and then the total is divided(/) with To currency value.
                ConvertedValue = (double.Parse(cmbFromCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbToCurrency.SelectedValue.ToString());

                //Show the label converted currency and converted currency name.
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }
        //This method is used to clear all the controls input which user entered
        private void ClearControls()
        {
            try
            {
                //Clear amount textbox text
                txtCurrency.Text = string.Empty;

                //From currency combobox items count greater than 0
                if (cmbFromCurrency.Items.Count > 0)
                {
                    //Set from currency combobox selected item hint
                    cmbFromCurrency.SelectedIndex = 0;
                }

                //To currency combobox items count greater than 0
                if (cmbToCurrency.Items.Count > 0)
                {
                    //Set to currency combobox selected item hint
                    cmbToCurrency.SelectedIndex = 0;
                }

                //Clear a label text
                lblCurrency.Content = "";

                //Set focus on amount textbox
                txtCurrency.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regular Expression to add regex. Add library using System.Text.RegularExpressions;
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cmbFromCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbToCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbFromCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtAmount.Focus();
                    return;
                }
                else if (txtCurrencyName.Text == null || txtCurrencyName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter currency name", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtCurrencyName.Focus();
                    return;
                }
                else
                {   //Edit time and set that record Id in CurrencyId variable.
                    //Code to Update. If CurrencyId greater than zero than it is go for update.
                    if (CurrencyId > 0)
                    {
                        //Show the confirmation message
                        if (MessageBox.Show("Are you sure you want to update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dt = new DataTable();

                            //Update Query Record update using Id
                            cmd = new SqlCommand("UPDATE Currency_Master SET Amount = @Amount, CurrencyName = @CurrencyName WHERE Id = @Id", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Data updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    // Code to Save
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to save ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            //Insert query to Save data in the table
                            cmd = new SqlCommand("INSERT INTO Currency_Master(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Data saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    ClearMaster();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearMaster();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // #endregion
        public void GetData()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(stringConnection))
                {
                    sqlConnection.Open();

                    string query = "SELECT * FROM Currency_Master";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dgvCurrency.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            dgvCurrency.ItemsSource = null;
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Method is used to clear all the input which user entered in currency master tab
        private void ClearMaster()
        {
            try
            {
                txtAmount.Text = string.Empty;
                txtCurrencyName.Text = string.Empty;
                btnSave.Content = "Save";
                GetData();
                CurrencyId = 0;
                BindCurrency();
                txtAmount.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //DataGrid selected cell changed event
        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                //Create object for DataGrid
                DataGrid grd = (DataGrid)sender;

                //Create an object for DataRowView
                DataRowView row_selected = grd.CurrentItem as DataRowView;

                //If row_selected is not null
                if (row_selected != null)
                {
                    //dgvCurrency items count greater than zero
                    if (dgvCurrency.Items.Count > 0)
                    {
                        if (grd.SelectedCells.Count > 0)
                        {
                            //Get selected row id column value and set it to the CurrencyId variable
                            CurrencyId = Int32.Parse(row_selected["Id"].ToString());

                            //DisplayIndex is equal to zero in the Edited cell
                            if (grd.SelectedCells[0].Column.DisplayIndex == 0)
                            {
                                //Get selected row amount column value and set to amount textbox
                                txtAmount.Text = row_selected["Amount"].ToString();

                                //Get selected row CurrencyName column value and set it to CurrencyName textbox
                                txtCurrencyName.Text = row_selected["CurrencyName"].ToString();
                                btnSave.Content = "Update";     //Change save button text Save to Update
                            }

                            //DisplayIndex is equal to one in the deleted cell
                            if (grd.SelectedCells[0].Column.DisplayIndex == 1)
                            {
                                //Show confirmation dialog box
                                if (MessageBox.Show("Are you sure you want to delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    mycon();
                                    DataTable dt = new DataTable();

                                    //Execute delete query to delete record from table using Id
                                    cmd = new SqlCommand("DELETE FROM Currency_Master WHERE Id = @Id", con);
                                    cmd.CommandType = CommandType.Text;

                                    //CurrencyId set in @Id parameter and send it in delete statement
                                    cmd.Parameters.AddWithValue("@Id", CurrencyId);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    MessageBox.Show("Data deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClearMaster();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}