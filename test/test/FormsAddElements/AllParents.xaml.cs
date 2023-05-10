using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using test.DataBaseClasses;
using static test.DataBase;

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для AllParents.xaml
    /// </summary>
    public partial class AllParents : Window
    {
        Parents selectedItem = new Parents();
        public AllParents()
        {
            InitializeComponent();
        }
        private void ButtonsVisible()
        {
            ButtonUpdate.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAdd.IsEnabled = true;
        }
        public async void SnackBar(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.Message.Content = text;
                SnackbarOne.IsActive = true;
            });
            await Task.Delay(2500);
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.IsActive = false;
            });
        }
        private void UpdateData()
        {
            using (var context = new DormContext())
            {
                var par = context.Parents.ToList();
                TestView.ItemsSource = par;
            }
        }
        public ObservableCollection<Parents> FilteredItems { get; set; } = new ObservableCollection<Parents>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                try
                {
                    Parents par = new Parents
                    {
                        Mother = TextChecker.CheckCyrillic(TextBoxMother.Text),
                        Father = TextChecker.CheckCyrillic(TextBoxFather.Text),
                        Marriage = CheckBoxStatusMarriage.IsChecked.Value
                    };
                    context.Parents.Add(par);
                    context.SaveChanges();
                    SnackBar("Добавлена новая запись");
                    UpdateData();
                    TextBoxMother.Text = null;
                    TextBoxFather.Text = null;
                    CheckBoxStatusMarriage.IsChecked = false;
                }
                catch (Exception asd)
                {
                    SnackBar("Неверные данные");
                }
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                try
                {
                    if (selectedItem != null)
                    {
                        selectedItem.Mother = TextBoxMother.Text;
                        selectedItem.Father = TextBoxFather.Text;
                        selectedItem.Marriage = CheckBoxStatusMarriage.IsChecked.Value;
                    }
                    context.Parents.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    TextBoxMother.Text = null;
                    TextBoxFather.Text = null;
                    CheckBoxStatusMarriage.IsChecked = false;
                    UpdateData();
                    selectedItem = null;
                }
                catch (Exception asd)
                {
                    SnackBar("Неверное значение");
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxMother.Text = null;
            TextBoxFather.Text = null;
            CheckBoxStatusMarriage.IsChecked = false;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Parents.Remove(selectedItem);
                context.SaveChanges();
                TextBoxMother.Text = null;
                TextBoxFather.Text = null;
                CheckBoxStatusMarriage.IsChecked = false;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private List<Parents> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Parents>();

                filtered.AddRange(context.Parents.Where(d =>
                                                    d.Mother.ToLower().Contains(filter) ||
                                                    d.Father.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Parents.Where(d =>
                                                    d.Id == filterNumber));
                
                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Parents)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxMother.Text = selectedItem.Mother;
                    TextBoxFather.Text = selectedItem.Father;
                    CheckBoxStatusMarriage.IsChecked = selectedItem.Marriage;
                    ButtonUpdate.IsEnabled = true;
                    ButtonCancel.IsEnabled = true;
                    ButtonDelete.IsEnabled = true;
                    ButtonAdd.IsEnabled = false;
                }
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            SnackBar("Данные из БД");
        }
    }
}
