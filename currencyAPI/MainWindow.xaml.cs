using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;

namespace currencyAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Create an object of root class
        Root val = new Root();

        //Root class is a main class. API return rates in a rates it return all currency name with Value.
        public class Root
        {
            //Get all record in rates and set in Rate class as currency name wise
            public Rate rates { get; set; }
            public long timestamp;
            public string license;
        }
        //Make sure API return value that name and where you want to store that name are same. Like in API get response INR then set it with INR name.
        public class Rate
        {
            public double INR { get; set; }
            public double JPY { get; set; }
            public double USD { get; set; }
            public double NZD { get; set; }
            public double EUR { get; set; }
            public double CAD { get; set; }
            public double ISK { get; set; }
            public double PHP { get; set; }
            public double DKK { get; set; }
            public double CZK { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            GetValue();
        }

       
        private async void GetValue()
        {
            val = await GetDataGetMethod<Root>("https://openexchangerates.org/api/latest.json?app_id=69cb235f2fe74f03baeec270066587cf");
            BindCurrency();
        }

        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            var ss = new Root();
            try
            {
                //HttpClient class provides a base class for sending/receiving the HTTP requests/responses from a URL.
                using (var client = new HttpClient())
                {
                    //The timespan to wait before the request times out.
                    client.Timeout = TimeSpan.FromMinutes(1);

                    //HttpResponseMessage is a way of returning a message/data from your action.
                    HttpResponseMessage response = await client.GetAsync(url);

                    //Check API response status code ok
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Serialize the HTTP content to a string as an asynchronous operation.
                        var ResponceString = await response.Content.ReadAsStringAsync();


                        //JsonConvert.DeserializeObject to deserialize Json to a C#
                        var ResponceObject = JsonConvert.DeserializeObject<Root>(ResponceString);
                        return ResponceObject;  //Return API responce
                    }
                    return ss;
                }
            }
            catch
            {
                return ss;
            }
        }

        private void BindCurrency()
        {
            //Create an object Datatable
            DataTable dt = new DataTable();

            //Add display column in DataTable
            dt.Columns.Add("Text");

            //Add value column in DataTable
            dt.Columns.Add("Rate");

            //Add rows in Datatable with text and value. Set a value which fetch from API
            dt.Rows.Add("--SELECT--", 0);
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("NZD", val.rates.NZD);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("ISK", val.rates.ISK);
            dt.Rows.Add("PHP", val.rates.PHP);
            dt.Rows.Add("DKK", val.rates.DKK);
            dt.Rows.Add("CZK", val.rates.CZK);

            //Datatable data assign From currency Combobox
            cmbFromCurrency.ItemsSource = dt.DefaultView;

            //DisplayMemberPath property is used to display data in Combobox
            cmbFromCurrency.DisplayMemberPath = "Text";

            //SelectedValuePath property is used to set value in Combobox
            cmbFromCurrency.SelectedValuePath = "Rate";

            //SelectedIndex property is used for when bind Combobox it's default selected item is first
            cmbFromCurrency.SelectedIndex = 0;

            //All Property Set For To Currency Combobox As From Currency Combobox
            cmbToCurrency.ItemsSource = dt.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Rate";
            cmbToCurrency.SelectedIndex = 0;
        }

        //ClearControls used for clear all controls input which user entered
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }


        //Allow only integer in textbox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //Assign convert button click event
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Declare ConvertedValue with double DataType for store currency converted value
            double ConvertedValue;

            //Check amount textbox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //If amount textbox is Null or Blank then show this message box
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //After click on Messagebox OK set focus on amount textbox
                txtCurrency.Focus();
                return;
            }
            //Else if currency From is not selected or default text --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.Text == "--SELECT--")
            {
                //Then show message box
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on From Combobox
                cmbFromCurrency.Focus();
                return;
            }
            //Else if currency To is not selected or default text --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.Text == "--SELECT--")
            {
                //Then show message
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set focus on To Combobox
                cmbToCurrency.Focus();
                return;
            }

            //If From and To Combobox selects same value
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //Amount textbox value set in ConvertedValue. double.parse is used to change Datatype String To Double. Textbox text have String and ConvertedValue is double datatype
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Show in label converted currency and converted currency name. And ToString("N3") is used for placing 000 after dot(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //Calculation for currency converter is From currency value is multiplied(*) with amount textbox value and then that total is divided(/) with To currency value.                
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                //Show the label converted currency and converted currency name.
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        //Assign a clear button click event
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            //ClearControls method is used to clear all control values
            ClearControls();
        }

     
    }   
}

