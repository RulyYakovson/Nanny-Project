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
using BE;
using BL;
using System.ComponentModel;

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for ContractsWindow.xaml
    /// </summary>
    public partial class ContractsWindow : Window
    {
        Contract contractToAdd;
        Contract contractToRemove;
        Contract contractToUpdate;
        List<string> errorsMessagesForAdd = new List<string>();
        List<string> errorsMessagesForUpdate = new List<string>();
        string[] PossibleFloor = { "(non)", "1", "2", "3", "4", "5", "7", "9" };
        string[] PossibleExperience = { "(non)", "1", "2", "3", "4", "5", "7", "9", "10" };


        public ContractsWindow()
        {
            InitializeComponent();
            contractToAdd = new Contract();
            contractToRemove = new Contract();
            contractToUpdate = new Contract();
            this.addContractPage.DataContext = contractToAdd; // The data context of the add contract page


            // Initialises the items in the combo boxes
            this.typeOfPaymentComboBox.ItemsSource = Enum.GetValues(typeof(Payment));
            this.childIdComboBox.ItemsSource = BlFactorySingleton.Instance.ChildrenWithoutNanny();
            this.childIdComboBox.DisplayMemberPath = "IdAndName";
            this.removeContractComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
            this.removeContractComboBox.DisplayMemberPath = "NumberAndSignatures";
            this.UpdateContractsComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
            this.UpdateContractsComboBox.DisplayMemberPath = "NumberAndSignatures";
            this.experienceComboBox.ItemsSource = PossibleExperience;
            this.floorComboBox.ItemsSource = PossibleFloor;
        }


        private void NanniesFilter_Button_Click(object sender, RoutedEventArgs e)
        {
            if (childIdComboBox.SelectedItem is Child)
            {

                if (this.filtersCheckBox.IsChecked == false)
                {
                    this.nannyIdListBox.ItemsSource = BlFactorySingleton.Instance.GetNannies();
                }
                if (this.filtersCheckBox.IsChecked == true)
                {
                    Child tempChild = this.childIdComboBox.SelectedItem as Child;
                    Mother tempMother = BlFactorySingleton.Instance.GetMothers(m => m.Id == tempChild.MotherId).ToList()[0];
                    bool floorB = true;
                    if (floorComboBox.SelectedIndex == -1 || floorComboBox.SelectedIndex == 0)
                    {
                        floorB = false;
                    }
                    bool experianceB = true;
                    if (experienceComboBox.SelectedIndex == -1 || experienceComboBox.SelectedIndex == 0)
                    {
                        experianceB = false;
                    }
                    int floorI = 0;
                    if (floorB)
                    {
                        floorI = int.Parse(floorComboBox.SelectedItem as string);
                    }
                    int experianceI = 0;
                    if (experianceB)
                    {
                        experianceI = int.Parse(experienceComboBox.SelectedItem as string);
                    }
                    this.nannyIdListBox.ItemsSource = BlFactorySingleton.Instance.NanniesByMotherNeeds(tempMother, daysAndHoursCheckBox.IsChecked, withElevatorCheckBox.IsChecked, floorB, floorI, experianceB, experianceI);
                }
            }
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
            if (contractToAdd.ContractSigned == false)
            {
                // Asks the user whether to enter the contract even though it is not signed
                MessageBoxResult boxResult = MessageBox.Show("The contract is not signed, insert it anyway?", "Question message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (boxResult == MessageBoxResult.No)
                {
                    return;
                }
            }
            // Adds the new contract
            try
            {
                if (this.childIdComboBox.SelectedItem is Child)
                {
                    contractToAdd.ChildId = (this.childIdComboBox.SelectedItem as Child).Id;

                }
                if (this.nannyIdListBox.SelectedItem is Nanny)
                {
                    contractToAdd.NannyId = (this.nannyIdListBox.SelectedItem as Nanny).Id;
                }
                BlFactorySingleton.Instance.AddContract(contractToAdd);
                MessageBox.Show("The contract was successfully added to the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                contractToAdd = new Contract();
                this.addContractPage.DataContext = contractToAdd;
                // Refreshes combo boxes
                this.childIdComboBox.ItemsSource = BlFactorySingleton.Instance.ChildrenWithoutNanny();
                this.removeContractComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                this.UpdateContractsComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                // Refreshes the "singed/unsigned Page"
                this.signed.IsChecked = false;
                this.unSigned.IsChecked = false;
                this.SignedUnsignedContractPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Warning the user
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to delete this contract from the database?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            try // Remove the contract
            {
                contractToRemove = removeContractComboBox.SelectedItem as Contract;
                BlFactorySingleton.Instance.RemoveContract(contractToRemove.ContractNumber);
                MessageBox.Show("The contract was successfully removed from the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                contractToRemove = new Contract();
                // Refreshes combo boxes
                this.childIdComboBox.ItemsSource = BlFactorySingleton.Instance.ChildrenWithoutNanny();
                this.removeContractComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                this.UpdateContractsComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                // Refreshes the "singed/unsigned Page"
                this.signed.IsChecked = false;
                this.unSigned.IsChecked = false;
                this.SignedUnsignedContractPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveContractComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Opens the button just if it's a good input
                if (comboBox.SelectedItem is Contract)
                {
                    this.removeButton.IsEnabled = true;
                    // Displays the details of the contract
                    contractToRemoveLabel.Content = (comboBox.SelectedItem as Contract).ToString();
                }
                else
                {
                    this.removeButton.IsEnabled = false;
                    contractToRemoveLabel.Content = "";
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
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to change the details of this contract?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            // Update the contract
            try
            {
                BlFactorySingleton.Instance.UpdateContract(contractToUpdate);
                MessageBox.Show("The contract was successfully updated", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                contractToUpdate = new Contract();
                this.updateContractPage.DataContext = contractToUpdate;
                // Refreshes combo boxes
                this.removeContractComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                this.UpdateContractsComboBox.ItemsSource = BlFactorySingleton.Instance.GetContracts();
                this.childIdComboBox.ItemsSource = BlFactorySingleton.Instance.ChildrenWithoutNanny();
                // Refreshes the "singed/unsigned Page"
                this.signed.IsChecked = false;
                this.unSigned.IsChecked = false;
                this.SignedUnsignedContractPage.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ContractsNumbersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Links the fields of the window to the selected contract
                if (comboBox.SelectedItem is Contract)
                {
                    contractToUpdate = UpdateContractsComboBox.SelectedItem as Contract;
                    this.updateContractPage.DataContext = contractToUpdate;
                }
                else
                {
                    contractToUpdate = new Contract();
                    this.updateContractPage.DataContext = contractToUpdate;
                }
            }
        }
        private void ErrorsManagment(object sender, ValidationErrorEventArgs e)
        {
            TabItem tabItem;
            if (sender is TabItem)
            {
                tabItem = sender as TabItem;
                // Saves and deletes the exceptions in and from the suitable list
                switch (tabItem.Name)
                {
                    case "addContractPage":
                        {
                            if (e.Action == ValidationErrorEventAction.Added)
                            {
                                errorsMessagesForAdd.Add(e.Error.Exception.Message);
                            }
                            else // Delete the exeption that fixed
                            {
                                errorsMessagesForAdd.Remove(e.Error.Exception.Message);
                            }
                            break;
                        }
                    case "aupdateContractPage":
                        {
                            if (e.Action == ValidationErrorEventAction.Added)
                            {
                                errorsMessagesForUpdate.Add(e.Error.Exception.Message);
                            }
                            else // Delete the exeption that fixed
                            {
                                errorsMessagesForUpdate.Remove(e.Error.Exception.Message);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void ChildIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.childIdComboBox.SelectedItem is Child)
            {
                this.nannyIdListBox.ItemsSource = BlFactorySingleton.Instance.GetNannies();
            }
            else
            {
                this.nannyIdListBox.ItemsSource = null;
            }
        }

        private void SignedUnsignedContractPage_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton;
            if (sender is RadioButton)
            {
                radioButton = sender as RadioButton;
                switch (radioButton.Name)
                {
                    case "signed":
                        this.SignedUnsignedContractPage.DataContext = BlFactorySingleton.Instance.GetContracts(c => c.ContractSigned == true);
                        break;
                    case "unSigned":
                        this.SignedUnsignedContractPage.DataContext = BlFactorySingleton.Instance.GetContracts(c => c.ContractSigned == false);                 
                        break;
                    default:
                        break;
                }
            }
        }

        private void ContractsByDistancePage_Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            this.ContractsByDistancePage_Button.Visibility = Visibility.Collapsed;
            this.ContractsByDistancePage_Label.Content = "<-----------------Please waite----------------->";
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<IGrouping<int, Contract>> result = e.Result as List<IGrouping<int, Contract>>;
            this.ContractsByDistancePage.DataContext = result;
            this.ContractsByDistancePage_Button.Visibility = Visibility.Visible;
            this.ContractsByDistancePage_Label.Content = "Contracts grouped by the distance between the nanny and the mother";
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<IGrouping<int, Contract>> relevantContracts = BlFactorySingleton.Instance.GetContractsByDistance(true).ToList();
            e.Result = relevantContracts;
        }

        private void ContractsByNannies_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.signedOnly.IsChecked == true)
            {
                this.ContractsByNanniesPage.DataContext = BlFactorySingleton.Instance.GetContractsByNannies(signedOnly: true);
            }
            else if (this.allContracts.IsChecked == true)
            {
                this.ContractsByNanniesPage.DataContext = BlFactorySingleton.Instance.GetContractsByNannies();
            }
        }
    }
}
