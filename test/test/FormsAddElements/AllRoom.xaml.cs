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
            using (var context = new DormContext())
            {
                var dorm = context.Dormitory.ToList();
                foreach (var item in dorm)
                {
                    ComboBoxDormitory.Items.Add(item.Id);
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
            ButtonUpdate.Visibility = Visibility.Hidden;
            ButtonCancel.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonAdd.Visibility = Visibility.Visible;
        }
        public ObservableCollection<Room> FilteredItems { get; set; } = new ObservableCollection<Room>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Dormitory dorm = context.Dormitory.FirstOrDefault(d => d.Id == Convert.ToInt32(ComboBoxDormitory.SelectedValue));
                if (dorm == null)
                {
                    SnackBar("Не верное общежитие");
                    return;
                }
                Room room = new Room
                {
                    RoomNumber = Convert.ToInt32(TextBoxNumber.Text),
                    Cost = Convert.ToInt32(TextBoxCost.Text),
                    DormitoryId = Convert.ToInt32(ComboBoxDormitory.SelectedValue),
                    Dormitory = dorm,
                    Living_space = Convert.ToInt32(TextBoxLivingSpace.Text),
                    Number_of_beds = Convert.ToInt32(TextBoxCountBeds.Text)
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
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    Dormitory dorm = context.Dormitory.FirstOrDefault(d => d.Id == Convert.ToInt32(ComboBoxDormitory.Text));
                    if (dorm == null)
                    {
                        SnackBar("Неверное общежитие");
                        return;
                    }
                    selectedItem.RoomNumber = Convert.ToInt32(TextBoxNumber.Text);
                    selectedItem.Cost = Convert.ToInt32(TextBoxCost.Text);
                    selectedItem.DormitoryId = dorm.Id;
                    selectedItem.Dormitory = dorm;
                    selectedItem.Living_space = Convert.ToInt32(TextBoxLivingSpace.Text);
                    selectedItem.Number_of_beds = Convert.ToInt32(TextBoxCountBeds.Text);

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

        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Room)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                Dormitory dorm = context.Dormitory.FirstOrDefault(d => d.Id == selectedItem.Id);
                if (selectedItem != null)
                {
                    TextBoxNumber.Text = selectedItem.RoomNumber.ToString();
                    TextBoxCost.Text = selectedItem.Cost.ToString();
                    ComboBoxDormitory.Text = dorm.Id.ToString();
                    TextBoxLivingSpace.Text = selectedItem.Living_space.ToString();
                    TextBoxCountBeds.Text = selectedItem.Number_of_beds.ToString();
                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonDelete.Visibility = Visibility.Visible;
                    ButtonAdd.Visibility = Visibility.Hidden;
                }
            }
                
        }
    }
}
