using System;
using System.Collections.Generic;
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
using static test.DataBase;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        public AddStudent()
        {
            InitializeComponent();
            using (DormContext db = new DormContext())
            {
                var Id_room = db.Room.ToList();
                foreach (var room in Id_room)
                {
                    ComboBoxRoom.Items.Add(room.Id);
                }
                var Id_Group = db.Group.ToList();
                foreach (var group in Id_Group)
                {
                    ComboBoxGroup.Items.Add(group.Id);
                }
                var Id_Parents = db.Parents.ToList();
                foreach (var parents in Id_Parents)
                {
                    ComboBoxParents.Items.Add(parents.Id);
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddStudent(TextBoxSurName.Text, TextBoxName.Text, TextBoxPatronymic.Text,TextBoxAddress.Text,CheckBoxStatusLearning.IsChecked.Value, TextBoxFormEducation.Text, TextBoxStatusResidence.Text,Convert.ToInt32(ComboBoxGroup.SelectedItem),Convert.ToInt32(ComboBoxParents.SelectedItem),Convert.ToInt32(ComboBoxRoom.SelectedItem));
        }
    }
}
