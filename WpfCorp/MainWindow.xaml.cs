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
    public static class TextBoxExtension
    {
        public static void OnlyDigits(this TextBox input, int length = 3)
        {
            bool notADigit = Regex.IsMatch(input.Text, "[^0-9]");

            if (notADigit || input.Text.Length > length)
            {
                input.Text = input.Text.Remove(input.Text.Length - 1);
                input.SelectionStart = input.Text.Length;
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty CompanyProperty;
        public CorporationViewModel corpPresenter
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
            corpPresenter.PropertyChanged += WhatsUp;
            PositionChoice.ItemsSource = Enum.GetValues(typeof(Level)).Cast<Level>();
        }

       
        private void AddButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (corpPresenter.SelectedItem == null)
            {
                MessageBox.Show("Не выбран отдел");
                return;
            }
            if (PositionChoice.SelectedIndex < 0 || FirstName.Text == "" || LastName.Text == "")
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }

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
            Selected.Text = corpPresenter.SelectedItem.Name; // потом это все убрать нахрен
        }

        private void PersonnelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //не нужна?
            
        }

        private void ResetButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            // работает через жопу, нужно по-другому
            if (Personnel.SelectedItem != null)
                ((EmployeeViewModel)Personnel.SelectedItem).Refresh();

            // не работает
            //var be = BindingOperations.GetBindingBase(EditForm, DataContextProperty);
            //EditForm.SetBinding(DataContextProperty, be);

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
                corpPresenter.SelectedItem.DismissEmployee((EmployeeViewModel)Personnel.SelectedItem);
        }

       
        private void ClearAddForm()
        {
            PositionChoice.SelectedIndex = -1;
            FirstName.Text = "";
            LastName.Text = "";
            Age.Text = "18";
        }

        private void MenuItemGenerateClick(object sender, RoutedEventArgs e)
        {
            corpPresenter.CreateRandomCorp(5, 5, 5);
        }

        private void WhatsUp(object sender, PropertyChangedEventArgs e)
        {
            Selected.Text = $"Изменилось поле {e.PropertyName}"; // баловство
        }

        private void AgeTextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).OnlyDigits();
        }

        private void MenuItemSaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            if ((bool)f.ShowDialog())
            {
                corpPresenter.Save(f.FileName);
                Selected.Text = $"Данные сохранены в файл {f.FileName}";
            }
        }
    }
}
