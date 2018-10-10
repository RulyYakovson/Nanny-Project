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
using System.ComponentModel;
using BE;
using BL;

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for MothersWindow.xaml
    /// </summary>
    public partial class MothersWindow : Window
    {
        Mother motherToAdd;
        Mother motherToRemove;
        Mother motherToUpdate;
        List<string> errorsMessagesForAdd = new List<string>();
        List<string> errorsMessagesForUpdate = new List<string>();
        int[] PossibleDistances = { 1, 2, 3, 5, 7, 10, 15, 20, 25, 30, 40, 50, 60, 75, 90 };


        public MothersWindow()
        {
            InitializeComponent();
            motherToAdd = new Mother();
            motherToRemove = new Mother();
            motherToUpdate = new Mother();
            this.addMotherPage.DataContext = motherToAdd; // The data context of the add mother page

            // Initialises the items in the combo boxes
            this.removeMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
            this.removeMotherComboBox.DisplayMemberPath = "IdAndName";
            this.updateMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
            this.updateMotherComboBox.DisplayMemberPath = "IdAndName";
            this.MothersInDistancePageComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
            this.MothersInDistancePageComboBox.DisplayMemberPath = "IdAndName";
            this.PossibleDistancesComboBox1.ItemsSource = PossibleDistances;
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
            // Adds the new mother
            try
            {
                if (motherToAdd.Address == "" && motherToAdd.LocationForNanny == "")
                {
                    throw new Exception("The fields: address and location for nanny must marked at least one");
                }
                motherToAdd.Address = addressTextBox.Text; // Inserts the address
                motherToAdd.LocationForNanny = locationForNannyTextBox.Text; // Inserts the location for nanny 
                BlFactorySingleton.Instance.AddMother(motherToAdd);
                MessageBox.Show("The mother was successfully added to the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                motherToAdd = new Mother();
                this.addMotherPage.DataContext = motherToAdd;
                addressTextBox.Text = "";
                locationForNannyTextBox.Text = "";
                this.removeMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the remove combo box
                this.updateMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the update combo box
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveMotherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Opens the button just if it's a good input
                if (comboBox.SelectedItem is Mother)
                {
                    // Displays the details of the nanny
                    motherToRemoveLabel.Content = (comboBox.SelectedItem as Mother).ToString();
                }
                else
                {
                    motherToRemoveLabel.Content = "";
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Warning the user
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to delete this mother from the database?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            try // Remove the mother
            {
                motherToRemove = removeMotherComboBox.SelectedItem as Mother;
                BlFactorySingleton.Instance.RemoveMother(motherToRemove.Id);
                MessageBox.Show("The mother was successfully removed from the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                motherToRemove = new Mother();
                this.removeMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the combo box
                this.updateMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the update combo box
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to change the details of this mother?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            // Update the mother
            try
            {
                if (motherToUpdate.Address == "" && motherToUpdate.LocationForNanny == "")
                {
                    throw new Exception("The fields: address and location for nanny must marked at least one");
                }
                motherToUpdate.Address = addressTextBox_U.Text; // Inserts the address
                motherToUpdate.LocationForNanny = locationForNannyTextBox_U.Text; // Inserts the location for nanny 
                BlFactorySingleton.Instance.UpdateMother(motherToUpdate);
                MessageBox.Show("The mother was successfully updated", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                motherToUpdate = new Mother();
                this.updateMotherPage.DataContext = motherToUpdate;
                addressTextBox_U.Text = "";
                locationForNannyTextBox_U.Text = "";
                this.removeMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the remove combo box
                this.updateMotherComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers(); // Refreshes the update combo box
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateMotherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Links the fields of the window to the selected mother
                if (comboBox.SelectedItem is Mother)
                {
                    motherToUpdate = updateMotherComboBox.SelectedItem as Mother;
                    this.updateMotherPage.DataContext = motherToUpdate;
                }
                else
                {
                    motherToUpdate = new Mother();
                    this.updateMotherPage.DataContext = motherToUpdate;
                }
            }
        }

        private void AddMotherPage_Error(object sender, ValidationErrorEventArgs e)
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

        private void CalculateDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            Mother mother = MothersInDistancePageComboBox.SelectedItem as Mother;
            int distance = (int)PossibleDistancesComboBox1.SelectedItem;
            object[] eventDetails = { mother, distance * 1000 };
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            this.MothersInDistancePageComboBox.Visibility = Visibility.Collapsed;
            this.PossibleDistancesComboBox1.Visibility = Visibility.Collapsed;
            this.CalculateDistanceButton.Visibility = Visibility.Collapsed;
             this.distanceWindowCaption.Content = "<------------------Searching------------------->";

            backgroundWorker.RunWorkerAsync(eventDetails);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Nanny> result = e.Result as List<Nanny>;
            this.nanniesByDistanceFromMother.DataContext = result;
            this.MothersInDistancePageComboBox.Visibility = Visibility.Visible;
            this.PossibleDistancesComboBox1.Visibility = Visibility.Visible;
            this.CalculateDistanceButton.Visibility = Visibility.Visible;
            this.distanceWindowCaption.Content = "Choose mother and the maximum distance for nanny";
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] getDetails = e.Argument as object[];
            Mother mother = getDetails[0] as Mother;
            int distance = (int)getDetails[1];
            List<Nanny> relevantNannies = BlFactorySingleton.Instance.GetNanniesByDistance(mother, distance).ToList();
            e.Result = relevantNannies;
        }
    }
}
