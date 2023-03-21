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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                var stud = context.Students.ToList();
                TestViewStudents.ItemsSource = stud;
                
            }
        }

        private void TestViewStudents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Student selectedItem = (Student)TestViewStudents.SelectedItem;
            MessageBox.Show(selectedItem.Name);
        }

        public ObservableCollection<Student> FilteredItems { get; set; } = new ObservableCollection<Student>();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text.ToLower();
            using (var context = new DormContext())
            {
                var klem = context.Students.Where(s => s.Name.ToLower().Contains(filterText)).ToList();
                FilteredItems.Clear();
                TestViewStudents.ItemsSource = klem;
            }
        }
    }

}
