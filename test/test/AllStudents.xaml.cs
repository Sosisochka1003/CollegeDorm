using Microsoft.EntityFrameworkCore;
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

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AllStudents.xaml
    /// </summary>
    public partial class AllStudents : Window
    {
        public AllStudents()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var gr = context.Group.ToList();
                foreach (var group in gr)
                {
                    combobox.Items.Add(group.Id);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                var stud = context.Student.ToList();
                TestViewStudents.ItemsSource = stud;
                
            }
            MessageBox.Show(combobox.Text);
        }

        private void TestViewStudents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Student selectedItem = (Student)TestViewStudents.SelectedItem;
        }

        public ObservableCollection<Student> FilteredItems { get; set; } = new ObservableCollection<Student>();
        //public ObservableCollection<Group> GFilteredItems { get; set; } = new ObservableCollection<Group>();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text.ToLower();
            using (var context = new DormContext())
            {
                var klem = context.Student.Where(s => 
                                                    s.Name.ToLower().Contains(filterText) ||
                                                    s.Surname.ToLower().Contains(filterText) ||
                                                    s.Patronymic.ToLower().Contains(filterText))
                                                .ToList();
                FilteredItems.Clear();
                TestViewStudents.ItemsSource = klem;
            }
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string filterText = e.AddedItems[0].ToString();
            using (var context = new DormContext())
            {
                var klem = context.Student.Where(s => s.GroupId.ToString().Contains(filterText)).ToList();
                FilteredItems.Clear();
                TestViewStudents.ItemsSource = klem;
            }
        }


    }

}
