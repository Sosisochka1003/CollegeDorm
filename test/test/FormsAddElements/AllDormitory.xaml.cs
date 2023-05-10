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
using System.Windows.Navigation;
using System.Windows.Shapes;
using test.DataBaseClasses;
using static test.DataBase;

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для AllDormitory.xaml
    /// </summary>
    public partial class AllDormitory : Window
    {
        Dormitory selectedItem = new Dormitory();
        DormContext ctx = new DormContext();
        List<Dormitory> dormitories;

        public AllDormitory()
        {
            InitializeComponent();
            dormitories = new List<Dormitory>( ctx.Dormitory.ToList());
            TestView.DataContext = dormitories;
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
                var dorm = context.Dormitory.ToList();
                TestView.ItemsSource = dorm;
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                try
                {
                    Dormitory dorm = new Dormitory
                    {
                        Address = TextBoxAddress.Text,
                        Numbers_of_rooms = TextChecker.CheckInt(TextBoxCountRoom.Text),
                    };
                    context.Dormitory.Add(dorm);
                    context.SaveChanges();
                    SnackBar("Добавлена новая запись");
                    UpdateData();
                    TextBoxAddress.Text = null;
                    TextBoxCountRoom.Text = null;
                }
                catch (Exception asdad)
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
                        selectedItem.Address = TextBoxAddress.Text;
                        selectedItem.Numbers_of_rooms = TextChecker.CheckInt(TextBoxCountRoom.Text);
                    }
                    context.Dormitory.Update(selectedItem);
                    context.SaveChanges();
                    SnackBar("Обновление данных");
                    ButtonsVisible();
                    TextBoxAddress.Text = null;
                    TextBoxCountRoom.Text = null;
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
            TextBoxAddress.Text = null;
            TextBoxCountRoom.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Dormitory.Remove(selectedItem);
                context.SaveChanges();
                TextBoxAddress.Text = null;
                TextBoxCountRoom.Text = null;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }
        public ObservableCollection<Dormitory> FilteredItems { get; set; } = new ObservableCollection<Dormitory>();

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private List<Dormitory> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Dormitory>();

                filtered.AddRange(context.Dormitory.Where(d =>
                                                    d.Address.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Dormitory.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.Numbers_of_rooms == filterNumber));

                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Dormitory)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxAddress.Text = selectedItem.Address;
                    TextBoxCountRoom.Text = selectedItem.Numbers_of_rooms.ToString();
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

        //private bool FilterAddress(Dormitory dorm, string address) => dorm.Address.ToLower().Contains(address.ToLower());

        private List<Dormitory> FilterAddress(string address)
        {
            return ctx.Dormitory.Where(x => x.Address.ToLower().Contains(address.ToLower())).ToList();
        }

        private List<Dormitory> FilterNumberOfRooms(int numberOfRooms)
        {
            return ctx.Dormitory.Where(x => x.Numbers_of_rooms == numberOfRooms).ToList();
        }

        private void ButtonFilter_Click(object sender, RoutedEventArgs e)
        {
            var address = TextBoxFilterAddress.Text.ToLower();
            var numberOfRoomsParsed = int.TryParse(address, out int numberOfRooms);

            var filtered = new List<List<Dormitory>>{
                address == "" ? FilterAddress(address) : ctx.Dormitory.ToList(),
                numberOfRoomsParsed ? FilterNumberOfRooms(numberOfRooms) : ctx.Dormitory.ToList()
            };

            var intersection = filtered
                .Skip(1)
                .Aggregate(
                    new HashSet<Dormitory>(filtered.First()),
                    (h, e) => { h.IntersectWith(e); return h; });


            TestView.ItemsSource = intersection;
        }
    }
}
