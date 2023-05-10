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
            try
            {
                var addspec = new Speciality
                {
                    Name = TextChecker.CheckCyrillic(TextBoxName.Text),
                };
                using (var context = new DormContext())
                {
                    context.Speciality.Add(addspec);
                    context.SaveChanges();
                }
                SnackBar("Специальность довалвена");
                TextBoxName.Text = null;
                UpdateData();
            }
            catch (Exception)
            {

                SnackBar("Неверные данные");
            }
        }

        private void ButtonsVisible()
        {
            ButtonUpdate.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAdd.IsEnabled = true;
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using(var context = new DormContext())
            {
                try
                {
                    if (selectedItem != null)
                    {
                        selectedItem.Name = TextChecker.CheckCyrillic(TextBoxName.Text);
                    }
                    context.Speciality.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    TextBoxName.Text = null;
                    UpdateData();
                    selectedItem = null;
                }
                catch (Exception)
                {
                    SnackBar("Неверные данные");
                }
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
                ButtonUpdate.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
            }
        }
        public ObservableCollection<Speciality> FilteredItems { get; set; } = new ObservableCollection<Speciality>();
        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private List<Speciality> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Speciality>();

                filtered.AddRange(context.Speciality.Where(d =>
                                                    d.Name.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Speciality.Where(d =>
                                                    d.Id == filterNumber));

                return filtered;
            }
        }

    }
}
