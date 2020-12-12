using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;
using WpfCorp.ViewModel;

namespace WpfCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PersonViewModel personView { get; }
        
        static readonly DependencyProperty CompanyProperty;
        CorporationViewModel corpPresenter
        {
            get { return (CorporationViewModel)GetValue(CompanyProperty); }
        
        
            set {SetValue(CompanyProperty, value); }
        }

        static MainWindow()
        {
            CompanyProperty = DependencyProperty.Register(
                "corpPresenter",
                typeof(CorporationViewModel),
                typeof(MainWindow));
        }
        
        public MainWindow()
        {
            InitializeComponent();

            corpPresenter = new CorporationViewModel();
            personView = (PersonViewModel)this.FindResource("personView");
            PositionChoice.ItemsSource = Enum.GetValues(typeof(Level)).Cast<Level>();
        }

        private void AddButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (AddFormInError())
                return;
            if (corpPresenter.SelectedItem == null)
            {
                MessageBox.Show("Не выбран отдел!");
                return;
            }
            if (PositionChoice.SelectedIndex < 0 )
            {
                MessageBox.Show("Не выбрана должность!");
                return;
            }

            Level position = (Level)PositionChoice.SelectedValue;

            try
            {
                corpPresenter.SelectedItem.RecruitPerson(personView.Person, position);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
           
            ResetAddForm();
        }

        private void ClearButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ResetAddForm();

        }
         
        private void ResetButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((EmployeeViewModel)Personnel.SelectedItem)?.Refresh();

        }

        private void ModifyButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NewFirstName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            NewLastName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            NewAge.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void DeleteButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Personnel.SelectedItem != null)
                try
                {
                    corpPresenter.SelectedItem?.DismissEmployee((EmployeeViewModel)Personnel.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
       
       
        private void MenuItemGenerateClick(object sender, RoutedEventArgs e)
        {
            corpPresenter.CreateRandomCorp(5, 5, 10);
        }
                
       
        private void MenuItemSaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            if ((bool)f.ShowDialog())
            {
                try
                {
                    corpPresenter.Save(f.FileName);
                    StatusMessage.Text = $"Данные сохранены в файл {f.FileName} в " + DateTime.Now.ToShortTimeString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MenuItemOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if ((bool)f.ShowDialog())
            {
                try
                {
                    corpPresenter.Load(f.FileName);
                    StatusMessage.Text = $"Данные загружены из файла {f.FileName} в " + DateTime.Now.ToShortTimeString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DeleteDepartmentMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            corpPresenter.DeleteSelection();
        }

        private void AddDepartmentMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            corpPresenter.CreateDepartment();
        }

        private bool AddFormInError()
        {
            return (System.Windows.Controls.Validation.GetHasError(FirstName) ||
                    System.Windows.Controls.Validation.GetHasError(LastName) ||
                    System.Windows.Controls.Validation.GetHasError(Age));
        }

        private void ResetAddForm()
        {
            personView.SetDefaults();
            PositionChoice.SelectedIndex = -1;

        }
        private void AgeTextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).OnlyDigits();
        }
    }
}
