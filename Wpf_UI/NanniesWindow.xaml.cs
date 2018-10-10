using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BE;
using BL;

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for Nanny.xaml
    /// </summary>
    public partial class NanniesWindow : Window
    {
        Nanny nannyToAdd;
        Nanny nannyToRemove;
        Nanny nannyToUpdate;
        List<string> errorsMessagesForAdd = new List<string>();
        List<string> errorsMessagesForUpdate = new List<string>();

        public NanniesWindow()
        {
            InitializeComponent();
            nannyToAdd = new Nanny
            {
                DateOfBirth = DateTime.Now.AddYears(-18) // The youngest age allowed
            };
            nannyToRemove = new Nanny();
            nannyToUpdate = new Nanny();
            this.addNannyPage.DataContext = nannyToAdd; // The data context of the add nanny page

            // Initialises the items in the combo boxes
            this.typeOfPaymentComboBox.ItemsSource = Enum.GetValues(typeof(Payment));
            this.vacationDaysComboBox.ItemsSource = Enum.GetValues(typeof(VacationDaysBy));
            this.typeOfPaymentComboBox_U.ItemsSource = Enum.GetValues(typeof(Payment));
            this.vacationDaysComboBox_U.ItemsSource = Enum.GetValues(typeof(VacationDaysBy));
            this.removeNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies();
            this.removeNannyComboBox.DisplayMemberPath = "IdAndName";
            this.updateNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies();
            this.updateNannyComboBox.DisplayMemberPath = "IdAndName";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorsMessagesForAdd.Any()) // If there is some exception
            {
                string message = "You have errors in entering the data:";
                foreach (var item in errorsMessagesForAdd)
                    message += "\n" + item;
                MessageBox.Show(message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Adds the new nanny
            try
            {
                nannyToAdd.Address = addressTextBox.Text; // Inserts the address
                BlFactorySingleton.Instance.AddNanny(nannyToAdd);
                MessageBox.Show("The nanny was successfully added to the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                nannyToAdd = new Nanny();
                this.addNannyPage.DataContext = nannyToAdd;
                addressTextBox.Text = "";
                this.removeNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the remove combo box
                this.updateNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the update combo box
                // Refreshes the "nannies Grouping Page"
                this.minAge.IsChecked = false;
                this.maxAge.IsChecked = false;
                this.nanniesGroupingPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Warning the user
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to delete this nanny from the database?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            try // Remove the nanny
            {
                nannyToRemove = removeNannyComboBox.SelectedItem as Nanny;
                BlFactorySingleton.Instance.RemoveNanny(nannyToRemove.Id);
                MessageBox.Show("The nanny was successfully removed from the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                nannyToRemove = new Nanny();
                this.removeNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the combo box
                this.updateNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the update combo box
                // Refreshes the "nannies Grouping Page"
                this.minAge.IsChecked = false;
                this.maxAge.IsChecked = false;
                this.nanniesGroupingPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveNannyCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Opens the button just if it's a good input
                if (comboBox.SelectedItem is Nanny)
                {
                    // Displays the details of the nanny
                    nannyToRemoveLabel.Content = (comboBox.SelectedItem as Nanny).ToString();
                }
                else
                {
                    nannyToRemoveLabel.Content = "";
                }
            }
        }

        private void UpdateNannyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Links the fields of the window to the selected nanny
                if (comboBox.SelectedItem is Nanny)
                {
                    nannyToUpdate = updateNannyComboBox.SelectedItem as Nanny;
                    this.updateNannyPage.DataContext = nannyToUpdate;
                }
                else
                {
                    nannyToUpdate = new Nanny();
                    this.updateNannyPage.DataContext = nannyToUpdate;
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorsMessagesForUpdate.Any()) // If there is some exception
            {
                string message = "You have errors in entering the data:";
                foreach (var item in errorsMessagesForUpdate)
                    message += "\n" + item;
                MessageBox.Show(message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Warning the user
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to change the details of this nanny?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            // Update the nanny
            try
            {
                nannyToAdd.Address = addressTextBox_U.Text; // Inserts the address
                BlFactorySingleton.Instance.UpdateNanny(nannyToUpdate);
                MessageBox.Show("The nanny was successfully updated", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                nannyToUpdate = new Nanny();
                this.updateNannyPage.DataContext = nannyToUpdate;
                this.removeNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the remove combo box
                this.updateNannyComboBox.ItemsSource = BlFactorySingleton.Instance.GetNannies(); // Refreshes the update combo box
                addressTextBox_U.Text = "";
                // Refreshes the "nannies Grouping Page"
                this.minAge.IsChecked = false;
                this.maxAge.IsChecked = false;
                this.nanniesGroupingPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNannyPage_Error(object sender, ValidationErrorEventArgs e)
        {
            // Saves the new exception in the suitable list
            if (e.Action == ValidationErrorEventAction.Added)
            {
                errorsMessagesForAdd.Add(e.Error.Exception.Message);
            }
            else // Delete the exeption that fixed
            {
                errorsMessagesForAdd.Remove(e.Error.Exception.Message);
            }
        }

        private void UpdateNannyPage_Error(object sender, ValidationErrorEventArgs e)
        {
            // Saves the new exception in the suitable list
            if (e.Action == ValidationErrorEventAction.Added)
            {
                errorsMessagesForUpdate.Add(e.Error.Exception.Message);
            }
            else // Delete the exeption that fixed
            {
                errorsMessagesForUpdate.Remove(e.Error.Exception.Message);
            }
        }

        private void GroupingRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton;
            if (sender is RadioButton)
            {
                radioButton = sender as RadioButton;
                switch (radioButton.Name)
                {
                    case "minAge":
                        this.nanniesGroupingPage.DataContext = BlFactorySingleton.Instance.GetNanniesByChildrenAge(true, true);
                        break;
                    case "maxAge":
                        this.nanniesGroupingPage.DataContext = BlFactorySingleton.Instance.GetNanniesByChildrenAge(false, true);
                        break;
                    default:
                        break;
                }
            }

        }

        private void NanniesByVacationDays_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.tamat.IsChecked == true)
            {
                this.nanniesByVacationDaysPage.DataContext = BlFactorySingleton.Instance.NanniesVacationDaysByTamat();
            }
            else if (this.official.IsChecked == true)
            {
                this.nanniesByVacationDaysPage.DataContext = BlFactorySingleton.Instance.NanniesVacationDaysByMinistryOfEducation();
            }
        }
    }
}
