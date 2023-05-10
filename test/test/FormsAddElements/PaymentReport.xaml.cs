using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using test.DataBaseClasses;
using static test.DataBase;

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для PaymentReport.xaml
    /// </summary>
    public partial class PaymentReport : Window
    {
        public PaymentReport()
        {
            InitializeComponent();
        }

        private void Paymentreport()
        {

            using (var context = new DormContext())
            {
                foreach (var item in context.Paymentl.ToList())
                {
                    context.Paymentl.Remove(item);
                }
                var payment = from student in context.Student
                              join room in context.Room on student.RoomId equals room.Id
                              join document in context.Document on student.Id equals document.StudentId
                              select new
                              {
                                  student.Surname,
                                  student.Name,
                                  room.RoomNumber,
                                  room.Cost,
                                  Pay = IsKvit(document.DName)
                              };
                foreach (var item in payment)
                {
                    //MessageBox.Show($"Фамилия:{item.Surname}\nИмя:{item.Name}\nНомер комнаты:{item.RoomNumber}\nСтоимость комнаты:{item.Cost}\nОплата комнаты:{item.Pay}");
                    Payment payment1 = new Payment
                    {
                        SurName = item.Surname,
                        Name = item.Name,
                        RoomNumber = item.RoomNumber,
                        Cost = (int)item.Cost,
                        isPaid = item.Pay
                    };
                    context.Paymentl.Add(payment1);
                }
                context.SaveChanges();
                var paykl = context.Paymentl.ToList();
                TestView.ItemsSource = paykl;
            }
            
        }

        static private string IsKvit(string Dname)
        {
            Regex regex = new Regex("[КкВвИиТтАаНнЦцИиЯя]*");
            MatchCollection matchCollection = regex.Matches(Dname);
            if (matchCollection[0].ToString() == Dname)
            {
                return "Оплачена";
            }
            return "Не оплачена";
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Paymentreport();
        }

        private List<Payment> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Payment>();

                filtered.AddRange(context.Paymentl.Where(d =>
                                                    d.SurName.ToLower().Contains(filter) ||
                                                    d.Name.ToLower().Contains(filter) ||
                                                    d.isPaid.ToLower() == filter));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Paymentl.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.Cost == filterNumber ||
                                                    d.RoomNumber == filterNumber));

                return filtered;
            }
        }
        public ObservableCollection<Payment> FilteredItems { get; set; } = new ObservableCollection<Payment>();

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
