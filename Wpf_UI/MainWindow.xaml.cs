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

namespace Wpf_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           //BL.BlFactorySingleton.Instance.Init();
        }

        private void Nannies_click(object sender, RoutedEventArgs e)
        {
            NanniesWindow nanniesWindow = new NanniesWindow();
            nanniesWindow.Show();
        }

        private void Mothers_click(object sender, RoutedEventArgs e)
        {
            MothersWindow mothersWindow = new MothersWindow();
            mothersWindow.Show();
        }

        private void Children_click(object sender, RoutedEventArgs e)
        {
            ChildrenWindow childrenWindow = new ChildrenWindow();
            childrenWindow.Show();
        }

        private void Contracts_click(object sender, RoutedEventArgs e)
        {
            ContractsWindow contractsWindow = new ContractsWindow();
            contractsWindow.Show();
        }

    }
}
