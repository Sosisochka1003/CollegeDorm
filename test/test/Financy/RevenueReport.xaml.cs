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
using test.FormsAddElements;
using static test.DataBase;

namespace test.Financy
{
    /// <summary>
    /// Логика взаимодействия для RevenueReport.xaml
    /// </summary>
    public partial class RevenueReport : Window
    {
        private int Total = 0;
        private int Now = 0;
        public RevenueReport()
        {
            InitializeComponent();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            revenueReport();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        public ObservableCollection<Revenue> FilteredItems { get; set; } = new ObservableCollection<Revenue>();

        private List<Revenue> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Revenue>();

                filtered.AddRange(context.Revenues.Where(d =>
                                                    d.isPaid.ToLower().Contains(filter) ||
                                                    d.isPaid.ToLower() == filter));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Revenues.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.RoomNumber == filterNumber ||
                                                    d.RoomCost == filterNumber ||
                                                    d.StudentID == filterNumber));

                return filtered;
            }
        }


        private void revenueReport()
        {
            Total = 0;
            Now = 0;
            using (var context = new DormContext())
            {
                foreach (var item in context.Revenues.ToList())
                {
                    context.Revenues.Remove(item);
                }
                foreach (var item in context.Room.ToList())
                {
                    Total += (int)item.Cost;
                }
                var revenue = from student in context.Student
                              join room in context.Room on student.RoomId equals room.Id
                              join document in context.Document on student.Id equals document.StudentId
                              join dormitory in context.Dormitory on room.DormitoryId equals dormitory.DId
                              select new
                              {
                                  dormitory.DId,
                                  room.RoomNumber,
                                  room.Cost,
                                  student.Id,
                                  Pay = PaymentReport.IsKvit(document.DName)
                              };
                foreach (var item in revenue)
                {
                    Revenue revenue1 = new Revenue
                    {
                        DormitoryID = item.DId,
                        RoomNumber = item.RoomNumber,
                        RoomCost = (int)item.Cost,
                        StudentID = item.Id,
                        isPaid = item.Pay
                    };
                    if (item.Pay == "Оплачена")
                    {
                        Now += (int)item.Cost;
                    }
                    context.Revenues.Add(revenue1);
                }
                context.SaveChanges();
                var revkl = context.Revenues.ToList();
                TestView.ItemsSource = revkl;
            }
            TextBoxTotal.Text = $"Общая цена: {Total}";
            TextBoxNow.Text = $"Заработанно: {Now}";
        }

    }
}
