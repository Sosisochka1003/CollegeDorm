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
    /// Логика взаимодействия для AllRoom.xaml
    /// </summary>
    public partial class AllRoom : Window
    {
        Room selectedItem = new Room();
        public AllRoom()
        {
            InitializeComponent();
            UpdateData();
            using (var context = new DormContext())
            {
                var dorm = context.Dormitory.ToList();
                foreach (var item in dorm)
                {
                    ComboBoxDormitory.Items.Add(item.DId);
                }
            }

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
                var room = context.Room.ToList();
                TestView.ItemsSource = room;
            }
        }
        private void ButtonsVisible()
        {
            ButtonUpdate.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAdd.IsEnabled = true;
        }
        public ObservableCollection<Room> FilteredItems { get; set; } = new ObservableCollection<Room>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Dormitory dorm = context.Dormitory.FirstOrDefault(d => d.DId == Convert.ToInt32(ComboBoxDormitory.SelectedValue));
                if (dorm == null)
                {
                    SnackBar("Не верное общежитие");
                    return;
                }
                try
                {
                    Room room = new Room
                    {
                        RoomNumber = TextChecker.CheckInt(TextBoxNumber.Text),
                        Cost = TextChecker.CheckInt(TextBoxCost.Text),
                        DormitoryId = Convert.ToInt32(ComboBoxDormitory.SelectedValue),
                        Dormitory = dorm,
                        Living_space = TextChecker.CheckInt(TextBoxLivingSpace.Text),
                        Number_of_beds = TextChecker.CheckInt(TextBoxCountBeds.Text)
                    };
                    context.Room.Add(room);
                    context.SaveChanges();
                    SnackBar("Добавлена новая запись");
                    UpdateData();
                    TextBoxNumber.Text = null;
                    TextBoxCost.Text = null;
                    ComboBoxDormitory.Text = null;
                    TextBoxLivingSpace.Text = null;
                    TextBoxCountBeds.Text = null;
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
                        Dormitory dorm = context.Dormitory.FirstOrDefault(d => d.DId == Convert.ToInt32(ComboBoxDormitory.Text));
                        if (dorm == null)
                        {
                            SnackBar("Неверное общежитие");
                            return;
                        }
                        selectedItem.RoomNumber = TextChecker.CheckInt(TextBoxNumber.Text);
                        selectedItem.Cost = TextChecker.CheckInt(TextBoxCost.Text);
                        selectedItem.DormitoryId = dorm.DId;
                        selectedItem.Dormitory = dorm;
                        selectedItem.Living_space = TextChecker.CheckInt(TextBoxLivingSpace.Text);
                        selectedItem.Number_of_beds = TextChecker.CheckInt(TextBoxCountBeds.Text);

                    }
                    context.Room.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    UpdateData();
                    TextBoxNumber.Text = null;
                    TextBoxCost.Text = null;
                    ComboBoxDormitory.Text = null;
                    TextBoxLivingSpace.Text = null;
                    TextBoxCountBeds.Text = null;
                    selectedItem = null;
                }
                catch (Exception asd)
                {
                    SnackBar("Неверные данные");
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNumber.Text = null;
            TextBoxCost.Text = null;
            ComboBoxDormitory.Text = null;
            TextBoxLivingSpace.Text = null;
            TextBoxCountBeds.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Room.Remove(selectedItem);
                context.SaveChanges();
                TextBoxNumber.Text = null;
                TextBoxCost.Text = null;
                ComboBoxDormitory.Text = null;
                TextBoxLivingSpace.Text = null;
                TextBoxCountBeds.Text = null;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SnackBar("Данные из БД");
            UpdateData();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            if (filtered.Count != 0)
            {
                TestView.ItemsSource = filtered;
                return;
            }
            UpdateData();
        }

        private List<Room> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Room>();


                var isNumber = int.TryParse(filter, out int filterNumber);

                filtered.AddRange(context.Room.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.RoomNumber == filterNumber ||
                                                    d.Cost == filterNumber ||
                                                    d.DormitoryId == filterNumber ||
                                                    d.Living_space == filterNumber ||
                                                    d.Number_of_beds== filterNumber));

                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Room)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null && selectedItem.DormitoryId != null)
                {
                    TextBoxNumber.Text = selectedItem.RoomNumber.ToString();
                    TextBoxCost.Text = selectedItem.Cost.ToString();
                    ComboBoxDormitory.Text = selectedItem.DormitoryId.ToString();
                    TextBoxLivingSpace.Text = selectedItem.Living_space.ToString();
                    TextBoxCountBeds.Text = selectedItem.Number_of_beds.ToString();
                    ButtonUpdate.IsEnabled = true;
                    ButtonCancel.IsEnabled = true;
                    ButtonDelete.IsEnabled = true;
                    ButtonAdd.IsEnabled = false;
                }
                else
                {
                    SnackBar("Ошибка");
                }
            }
                
        }
    }
}
