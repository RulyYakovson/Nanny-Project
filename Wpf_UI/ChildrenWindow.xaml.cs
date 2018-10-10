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

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for ChildrenWindow.xaml
    /// </summary>
    public partial class ChildrenWindow : Window
    {
        Child childToAdd;
        Child childToRemove;
        Child childToUpdate;
        List<string> errorsMessagesForAdd = new List<string>();
        List<string> errorsMessagesForUpdate = new List<string>();

        public ChildrenWindow()
        {
            InitializeComponent();

            childToAdd = new Child
            {
                DateOfBirth = DateTime.Now
            };
            childToRemove = new Child();
            childToUpdate = new Child();
            this.addChildPage.DataContext = childToAdd; // The data context of the add child page

            // Initialises the items in the combo boxes
            this.mothersIdComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
            this.mothersIdComboBox.DisplayMemberPath = "Id";
            this.removeChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
            this.removeChildComboBox.DisplayMemberPath = "IdAndName";
            this.updateChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
            this.updateChildComboBox.DisplayMemberPath = "IdAndName";

            // Initialises the data context of the grouping page
            this.childrenGroupingPage.DataContext = BlFactorySingleton.Instance.GetChildrenByMothers();
        }

        private void ErrorsManagement(object sender, ValidationErrorEventArgs e)
        {
            TabItem tabItem;
            if (sender is TabItem)
            {
                tabItem = sender as TabItem;
                // Saves and deletes the exceptions in and from the suitable list
                switch (tabItem.Name)
                {
                    case "addChildPage":
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
                    case "updateChildPage":
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
            // Adds the new child
            try
            {
                if (mothersIdComboBox.SelectedItem is Mother)
                {
                    childToAdd.MotherId = (mothersIdComboBox.SelectedItem as Mother).Id;
                }
                BlFactorySingleton.Instance.AddChild(childToAdd);
                MessageBox.Show("The child was successfully added to the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                childToAdd = new Child();
                this.addChildPage.DataContext = childToAdd;
                // Refreshes combo boxes
                this.mothersIdComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
                this.removeChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                this.updateChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                // Refreshes the data binding of the grouping children page
                this.childrenGroupingPage.DataContext = BlFactorySingleton.Instance.GetChildrenByMothers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveChildComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Opens the button just if it's a good input
                if (comboBox.SelectedItem is Child)
                {
                    // Displays the details of the child
                    childToRemoveLabel.Content = (comboBox.SelectedItem as Child).ToString();
                }
                else
                {
                    childToRemoveLabel.Content = "";
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Warning the user
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to delete this child from the database?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            try // Remove the child
            {
                childToRemove = removeChildComboBox.SelectedItem as Child;
                BlFactorySingleton.Instance.RemoveChild(childToRemove.Id);
                MessageBox.Show("The child was successfully removed from the database", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                childToRemove = new Child();
                // Refreshes combo boxes
                this.mothersIdComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
                this.removeChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                this.updateChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                // Refreshes the data binding of the grouping children page
                this.childrenGroupingPage.DataContext = BlFactorySingleton.Instance.GetChildrenByMothers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateChildComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                // Links the fields of the window to the selected child
                if (comboBox.SelectedItem is Child)
                {
                    childToUpdate = updateChildComboBox.SelectedItem as Child;
                    this.updateChildPage.DataContext = childToUpdate;
                    //this.motherIdTextBox_U.Content = 
                }
                else
                {
                    childToUpdate = new Child();
                    this.updateChildPage.DataContext = childToUpdate;
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
            MessageBoxResult boxResult = MessageBox.Show("Are you sure you want to change the details of this child?", "Warning message", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            // Update the child
            try
            {
                BlFactorySingleton.Instance.UpdateChild(childToUpdate);
                MessageBox.Show("The child was successfully updated", "Successfull message", MessageBoxButton.OK, MessageBoxImage.Information);
                childToUpdate = new Child();
                this.updateChildPage.DataContext = childToUpdate;
                // Refreshes combo boxes
                this.mothersIdComboBox.ItemsSource = BlFactorySingleton.Instance.GetMothers();
                this.removeChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                this.updateChildComboBox.ItemsSource = BlFactorySingleton.Instance.GetChildren();
                // Refreshes the data binding of the grouping children page
                this.childrenGroupingPage.DataContext = BlFactorySingleton.Instance.GetChildrenByMothers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChildrenWithoutNanny_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ChildrenWithoutNannyPage.DataContext = BlFactorySingleton.Instance.ChildrenWithoutNanny();
        }
    }
}
