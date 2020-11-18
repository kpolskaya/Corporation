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
using Corporation;
using WpfCorp.ViewModel;

namespace WpfCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Repository myCorp;
        CorporationViewModel corpPresenter;

        public MainWindow()
        {
            InitializeComponent();

            myCorp = new Repository(11, 5, 8);
            corpPresenter = new CorporationViewModel(myCorp.Board);
            DataContext = corpPresenter;

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //Selected.Text = corpPresenter.SelectedItem == null ? "Ничего не выбрано" :
            //   corpPresenter.SelectedItem.Name;
            //Personnel.ItemsSource = corpPresenter.SelectedItem.Staff;
        }
    }
}
