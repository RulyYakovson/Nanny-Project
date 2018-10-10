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
using System.Threading;
using System.Configuration;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for AutoCompleteAddressTextBox.xaml
    /// </summary>
    public partial class AutoCompleteAddressTextBox : UserControl
    {
        //static string API_KEY1 = ConfigurationManager.AppSettings["googleApiKey"];
        static string API_KEY = ConfigurationSettings.AppSettings.Get("googleApiKey");

        public AutoCompleteAddressTextBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteAddressTextBox), new PropertyMetadata(null));

        public static List<string> GetPlaceAutoComplete(string str)
        {
            List<string> result = new List<string>();
            var request = new GoogleMapsApi.Entities.PlaceAutocomplete.Request.PlaceAutocompleteRequest
            {
                ApiKey = API_KEY,
                Input = str
            };
            var response = GoogleMaps.PlaceAutocomplete.Query(request);
            foreach (var item in response.Results)
            {
                result.Add(item.Description);
            }
            return result;
        }

        void Run(object text)
        {
            if (text != null)
            {
                List<string> result = GetPlaceAutoComplete(text.ToString());
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
                Action<List<string>> action = SetListInvok;
                Dispatcher.BeginInvoke(action, new object[] { result });
            }
        }

        private void SetListInvok(List<string> list)
        {
            textComboBox.ItemsSource = null;
            if (list.Count > 0 && list[0].CompareTo(Text) != 0)
            {
                textComboBox.ItemsSource = list;
                textComboBox.IsDropDownOpen = true;
            }
            else
            {
                textComboBox.IsDropDownOpen = false;
            }
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            new Thread(Run).Start(Text);
        }

        private void TextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = textComboBox.SelectedItem;
            if (item != null)
            {
                Text = item.ToString();
                textComboBox.IsDropDownOpen = false;
            }
        }

        private void TextInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                textComboBox.Focus();
            }
        }

        private void TextComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (textComboBox.SelectedIndex == 0)
                    textInput.Focus();
            }
        }
    }
}
