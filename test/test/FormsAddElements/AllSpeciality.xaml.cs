using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using test.DataBaseClasses;
using static test.DataBase;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AllSpeciality.xaml
    /// </summary>
    public partial class AllSpeciality : Window
    {
        Speciality selectedItem = new Speciality();
        public AllSpeciality()
        {
            InitializeComponent();
        }
        public async void SnackBar(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.Message.Content = text;
                SnackbarOne.IsActive = true;
            });
            await Task.Delay(2000);
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.IsActive = false;
            });
        }
        private void UpdateData()
        {
            using (var context = new DormContext())
            {
                var spec = context.Speciality.ToList();
                TestView.ItemsSource = spec;
            }
        }
        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SnackBar("Данные из БД");
            UpdateData();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var addspec = new Speciality
            {
                Name = TextBoxName.Text,
            };
            using (var context = new DormContext())
            {
                context.Speciality.Add(addspec);
                context.SaveChanges();
            }
            SnackBar("Специальность довалвена");
            UpdateData();
        }

        private void ButtonsVisible()
        {
            ButtonUpdate.Visibility = Visibility.Hidden;
            ButtonCancel.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonAdd.Visibility = Visibility.Visible;
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using(var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    selectedItem.Name = TextBoxName.Text;
                }
                context.Speciality.Update(selectedItem);
                context.SaveChanges();
                SnackBar("Обновление данных");
                ButtonsVisible();
                TextBoxName.Text = null;
                UpdateData();
                selectedItem = null;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using(var context = new DormContext())
            {
                context.Speciality.Remove(selectedItem);
                context.SaveChanges();
                TextBoxName.Text = null;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Speciality)TestView.SelectedItem;
            if (selectedItem != null)
            {
                TextBoxName.Text = selectedItem.Name;
                ButtonUpdate.Visibility = Visibility.Visible;
                ButtonCancel.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Visible;
                ButtonAdd.Visibility = Visibility.Hidden;
            }
        }
        public ObservableCollection<Speciality> FilteredItems { get; set; } = new ObservableCollection<Speciality>();
        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text.ToLower();
            using (var context = new DormContext())
            {
                if (filterText != null)
                {
                    var klem = context.Speciality.Where(s =>
                                                    s.Name.ToLower().Contains(filterText))
                                                .ToList();
                    FilteredItems.Clear();
                    TestView.ItemsSource = klem;
                }
            }
        }
    }
}
