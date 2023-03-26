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
        }

        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddRoom(Convert.ToInt32(textbox_number.Text),Convert.ToDecimal(textbox_name_price.Text));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AllStudents win1 = new AllStudents();
            win1.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (DormContext db = new DormContext())
            {
                //Speciality spec1 = new Speciality { Name = "mocha" };
                //Group group1 = new Group { Id_spec = 1, Number = "klema1"};
                //Parents par1 = new Parents { Mother = "нету", Father = "нету", Marriage = true };
                //Student student1 = new Student
                //{
                //    RoomId = 1,
                //    Surname = "Гапченко",
                //    Name = "Сергей",
                //    Patronymic = "Сергеевич",
                //    Home_Address = "korolev",
                //    Status_learning = true,
                //    Form_of_education = "очно",
                //    Status_residence = "klema",
                //    Id_group = 1,
                //    Id_parents = 1,
                //};
                //db.Speciality.Add(spec1);
                //db.Group.Add(group1);
                //db.Parents.Add(par1);
                //db.Students.Add(student1);
                db.SaveChanges();

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddStudent addStudent = new AddStudent();
            addStudent.Show();
        }
    }

}
