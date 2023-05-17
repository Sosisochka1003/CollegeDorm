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
    /// Логика взаимодействия для AllHardInventoryRoom.xaml
    /// </summary>
    public partial class AllHardInventoryRoom : Window
    {
        HardInventoryRoom selectedItem = new HardInventoryRoom();
        public AllHardInventoryRoom()
        {
            InitializeComponent();
            UpdateData();
            using (var context = new DormContext())
            {
                var room = context.Room.ToList();
                foreach (var item in room)
                {
                    ComboBoxRoom.Items.Add(item.Id);
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
                var soft = context.HardInventoryRoom.ToList();
                TestView.ItemsSource = soft;
            }
        }
        public ObservableCollection<HardInventoryRoom> FilteredItems { get; set; } = new ObservableCollection<HardInventoryRoom>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Room room = context.Room.FirstOrDefault(s => s.Id == Convert.ToInt32(ComboBoxRoom.SelectedValue));
                if (room == null)
                {
                    SnackBar("Неверная комната");
                    return;
                }
                try
                {
                    HardInventoryRoom hard = new HardInventoryRoom
                    {
                        Name = TextBoxName.Text,
                        RoomId = room.Id,
                        Room = room,
                        Date_purchase = DateOnly.FromDateTime((DateTime)DatePickerDatePurchase.SelectedDate),
                    };
                    context.HardInventoryRoom.Add(hard);
                    context.SaveChanges();
                    SnackBar("Добавлена новая запись");
                    UpdateData();
                    TextBoxName.Text = null;
                    ComboBoxRoom.Text = null;
                    DatePickerDatePurchase.Text = null;
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
                        Room room = context.Room.FirstOrDefault(r => r.Id == Convert.ToInt32(ComboBoxRoom.SelectedValue));
                        if (room == null)
                        {
                            SnackBar("Неверная комната");
                            return;
                        }
                        selectedItem.Name = TextBoxName.Text;
                        selectedItem.RoomId = room.Id;
                        selectedItem.Room = room;
                        selectedItem.Date_purchase = DateOnly.FromDateTime((DateTime)DatePickerDatePurchase.SelectedDate);
                    }
                    context.HardInventoryRoom.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    TextBoxName.Text = null;
                    ComboBoxRoom.Text = null;
                    UpdateData();
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
            TextBoxName.Text = null;
            ComboBoxRoom.Text = null;
            DatePickerDatePurchase.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.HardInventoryRoom.Remove(selectedItem);
                context.SaveChanges();
                TextBoxName.Text = null;
                ComboBoxRoom.Text = null;
                DatePickerDatePurchase.Text = null;
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

        private List<HardInventoryRoom> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<HardInventoryRoom>();

                filtered.AddRange(context.HardInventoryRoom.Where(d =>
                                                    d.Name.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.HardInventoryRoom.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.RoomId == filterNumber));

                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (HardInventoryRoom)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxName.Text = selectedItem.Name;
                    ComboBoxRoom.Text = selectedItem.RoomId.ToString();
                    DatePickerDatePurchase.Text = selectedItem.Date_purchase.ToString();
                    ButtonUpdate.IsEnabled = true;
                    ButtonCancel.IsEnabled = true;
                    ButtonDelete.IsEnabled = true;
                    ButtonAdd.IsEnabled = false;
                }
            }
        }
    }
}
