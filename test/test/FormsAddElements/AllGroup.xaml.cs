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

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для AllGroup.xaml
    /// </summary>
    public partial class AllGroup : Window
    {
        Group selectedItem = new Group();
        public AllGroup()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var spec = context.Speciality.ToList();
                foreach (var item in spec)
                {
                    ComboBoxSpeciality.Items.Add(item.Name);
                }
            }
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
                var group = context.Group.ToList();
                TestView.ItemsSource = group;
            }
        }
        public ObservableCollection<Group> FilteredItems { get; set; } = new ObservableCollection<Group>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Speciality spec = context.Speciality.FirstOrDefault(s => s.Name == ComboBoxSpeciality.SelectedValue);
                if (spec == null)
                {
                    SnackBar("Не верная специальность");
                    return;
                }
                try
                {
                    Group group = new Group
                    {
                        Number = TextBoxName.Text,
                        SpecialityId = spec.Id,
                        Speciality = spec
                    };
                    context.Group.Add(group);
                    context.SaveChanges();
                    SnackBar("Добавлена новая запись");
                    UpdateData();
                    TextBoxName.Text = null;
                    ComboBoxSpeciality.Text = null;
                }
                catch (Exception asd)
                {
                    SnackBar("Неверное значение");
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
                        Speciality spec = context.Speciality.FirstOrDefault(s => s.Name == ComboBoxSpeciality.SelectedValue);
                        if (spec == null)
                        {
                            SnackBar("Неверная специальность");
                            return;
                        }
                        selectedItem.Number = TextBoxName.Text;
                        selectedItem.SpecialityId = spec.Id;
                        selectedItem.Speciality = spec;
                    }
                    context.Group.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    TextBoxName.Text = null;
                    ComboBoxSpeciality.Text = null;
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
            TextBoxName.Text= null;
            ComboBoxSpeciality.Text= null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Group.Remove(selectedItem);
                context.SaveChanges();
                TextBoxName.Text = null;
                ComboBoxSpeciality.Text= null;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            SnackBar("Данные из БД");
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private List<Group> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Group>();

                filtered.AddRange(context.Group.Where(d =>
                                                    d.Number.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Group.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.SpecialityId == filterNumber));

                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Group)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    Speciality speciality = context.Speciality.FirstOrDefault(s => s.Id == selectedItem.SpecialityId);
                    TextBoxName.Text = selectedItem.Number;
                    ComboBoxSpeciality.Text = speciality.Name;
                    ButtonUpdate.IsEnabled = true;
                    ButtonCancel.IsEnabled = true;
                    ButtonDelete.IsEnabled = true;
                    ButtonAdd.IsEnabled = false;


                }
            }
        }
    }
}
