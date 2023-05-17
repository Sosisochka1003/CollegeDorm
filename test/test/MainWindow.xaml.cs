using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using test.DataBaseClasses;
using test.Financy;
using test.FormsAddElements;
using static test.DataBase;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                context.Student.ToList();
            }

            if (!Settings1.Default.IsConnect)
            {
                ButtonAddStudent.IsEnabled = false;
                ButtonAddSpeciality.IsEnabled = false;
                ButtonAddGroup.IsEnabled = false;
                ButtonAddDormitory.IsEnabled = false;
                ButtonAddParents.IsEnabled = false;
                ButtonAddRoom.IsEnabled = false;
                ButtonAddDocument.IsEnabled = false;
                ButtonAddStudentSoft.IsEnabled = false;
                ButtonAddHardInventoryRoom.IsEnabled = false;
            }
            else if (Settings1.Default.IsConnect)
            {
                ButtonAddStudent.IsEnabled = true;
                ButtonAddSpeciality.IsEnabled = true;
                ButtonAddGroup.IsEnabled = true;
                ButtonAddDormitory.IsEnabled = true;
                ButtonAddParents.IsEnabled = true;
                ButtonAddRoom.IsEnabled = true;
                ButtonAddDocument.IsEnabled = true;
                ButtonAddStudentSoft.IsEnabled = true;
                ButtonAddHardInventoryRoom.IsEnabled = true;
            }
        }
        
        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AllStudents stud = new AllStudents();
            stud.Show();
        }

        private void ButtonAddSpeciality_Click(object sender, RoutedEventArgs e)
        {
            AllSpeciality speciality = new AllSpeciality();
            speciality.Show();
        }

        private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            AllGroup group = new AllGroup();
            group.Show();
        }

        private void ButtonAddDormitory_Click(object sender, RoutedEventArgs e)
        {
            AllDormitory dorm = new AllDormitory();
            dorm.Show();
        }

        private void ButtonAddParents_Click(object sender, RoutedEventArgs e)
        {
            AllParents parents = new AllParents();
            parents.Show();
        }

        private void ButtonAddRoom_Click(object sender, RoutedEventArgs e)
        {
            AllRoom room = new AllRoom();
            room.Show();
        }

        private void ButtonAddDocument_Click(object sender, RoutedEventArgs e)
        {
            AllDocuments document = new AllDocuments();
            document.Show();
        }

        private void ButtonAddStudentSoft_Click(object sender, RoutedEventArgs e)
        {
            AllStudentSoftStock allStudentSoftStock = new AllStudentSoftStock();
            allStudentSoftStock.Show();
        }

        private void ButtonAddHardInventoryRoom_Click(object sender, RoutedEventArgs e)
        {
            AllHardInventoryRoom allHardInventoryRoom = new AllHardInventoryRoom();
            allHardInventoryRoom.Show();
        }

        private void ButtonConnectBD_Click(object sender, RoutedEventArgs e)
        {
            ConnectBD connectBD = new ConnectBD();
            connectBD.Show();
        }

        private void ButtonPaymentReport_Click(object sender, RoutedEventArgs e)
        {
            PaymentReport paymentReport = new PaymentReport();
            paymentReport.Show();
        }

        private void ButtonRevenueReport_Click(object sender, RoutedEventArgs e)
        {
            RevenueReport revenueReport = new RevenueReport();
            revenueReport.Show();
        }
    }

}
