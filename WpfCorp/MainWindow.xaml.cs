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
        //Person currentPerson;

        public MainWindow()
        {
            InitializeComponent();

            myCorp = new Repository(11, 5, 8);
            corpPresenter = new CorporationViewModel(myCorp.Board);
            //currentPerson = new Person("", "", 0);
            DataContext = corpPresenter;
            PositionChoice.ItemsSource = Enum.GetValues(typeof(Level)).Cast<Level>();
        }

       
        private void AddButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Level position = (Level)PositionChoice.SelectedValue;
            corpPresenter.SelectedItem.RecruitPerson(FirstName.Text, LastName.Text, uint.Parse(Age.Text), position);
            ClearAddForm();
            
        }

        private void ClearButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAddForm();

        }

        private void CompanyTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //не нужна?
        }

        private void PersonnelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //не нужна?
            
        }

        private void ResetButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            ((EmployeeViewModel)Personnel.SelectedItem).Refresh(); 
           
        }

        private void ModifyButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((EmployeeViewModel)Personnel.SelectedItem).ApplyNewData(NewFirstName.Text, NewLastName.Text, uint.Parse(NewAge.Text));
        }

        private void DeleteButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            corpPresenter.SelectedItem.DismissEmployee((EmployeeViewModel)Personnel.SelectedItem);
        }

       
        private void ClearAddForm()
        {
            PositionChoice.SelectedIndex = -1;
            FirstName.Text = "";
            LastName.Text = "";
            Age.Text = "18";
        }
    }
}
