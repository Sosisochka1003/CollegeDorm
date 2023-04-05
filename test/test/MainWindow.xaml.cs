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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (DormContext db = new DormContext())
            {
                Speciality speciality = new Speciality() { Id = 1, Name = "Информациооные системы" };
                Speciality speciality1 = new Speciality() { Id = 2, Name = "Юристы" };
                Group group = new Group() { Id = 1, Number = "ИС1-20", SpecialityId = 1, Speciality = speciality };
                Group group1 = new Group() { Id = 2, Number = "Ю1-21", SpecialityId = 2, Speciality = speciality1 };
                Parents parents = new Parents() { Id = 1, Father = "Никита", Mother = "Миша", Marriage = true };
                Parents parents1 = new Parents() { Id = 2, Father = "За хлебом", Mother = "Женя", Marriage = false };
                Dormitory dormitory = new Dormitory() { Id = 1, Address = "Королёв", Numbers_of_rooms = 20 };
                Room room = new Room() { Id = 1, RoomNumber = 1, Cost = 3000, DormitoryId = 1, Dormitory = dormitory, Living_space = 30, Number_of_beds = 50 };
                db.Speciality.Add(speciality);
                db.Speciality.Add(speciality1);
                db.Group.Add(group);
                db.Group.Add(group1);
                db.Parents.Add(parents);
                db.Parents.Add(parents1);
                db.Dormitory.Add(dormitory);
                db.Room.Add(room);
                db.SaveChanges();

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
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

        private void ColorZone_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("ты клема");
        }

        private void ButtonConnectBD_Click(object sender, RoutedEventArgs e)
        {
            ConnectBD connectBD = new ConnectBD();
            connectBD.Show();
        }
    }

}
