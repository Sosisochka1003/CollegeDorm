using MaterialDesignThemes.Wpf;
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
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace test
{
    /// <summary>
    /// Логика взаимодействия для AllStudents.xaml
    /// </summary>
    public partial class AllStudents : Window
    {
        Student selectedItem = new Student();
        public AllStudents()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var gr = context.Group.ToList();
                foreach (var group in gr)
                {
                    ComboBoxGroup.Items.Add(group.Id);
                }
                var Id_room = context.Room.ToList();
                foreach (var room in Id_room)
                {
                    ComboBoxRoom.Items.Add(room.Id);
                }
                var Id_Parents = context.Parents.ToList();
                foreach (var parents in Id_Parents)
                {
                    ComboBoxParents.Items.Add(parents.Id);
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
                var stud = context.Student.ToList();
                TestViewStudents.ItemsSource = stud;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SnackBar("Данные из БД");
            UpdateData();
        }

        private void TestViewStudents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Student)TestViewStudents.SelectedItem;
            if (selectedItem != null)
            {
                TextBoxSurName.Text = selectedItem.Surname;
                TextBoxName.Text = selectedItem.Name;
                TextBoxPatronymic.Text = selectedItem.Patronymic;
                TextBoxAddress.Text = selectedItem.Home_Address;
                CheckBoxStatusLearning.IsChecked = selectedItem.Status_learning;
                ComboBoxRoom.SelectedValue = selectedItem.RoomId;
                ComboBoxStatusResidence.SelectedValue = selectedItem.Status_residence;
                ComboBoxFormEducation.SelectedValue = selectedItem.Form_of_education;
                ComboBoxGroup.SelectedValue = selectedItem.GroupId;
                ComboBoxParents.SelectedValue = selectedItem.ParentsId;
                ButtonUpdateStudent.IsEnabled = true;
                ButtonCancel.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
                ButtonAddStudent.IsEnabled = false;
            }
        }

        public ObservableCollection<Student> FilteredItems { get; set; } = new ObservableCollection<Student>();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestViewStudents.ItemsSource = filtered;
        }

        private List<Student> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Student>();

                filtered.AddRange(context.Student.Where(d =>
                                                    d.Surname.ToLower().Contains(filter) ||
                                                    d.Name.ToLower().Contains(filter) ||
                                                    d.Patronymic.ToLower().Contains(filter) ||
                                                    d.Home_Address.ToLower().Contains(filter) ||
                                                    d.Form_of_education.ToLower().Contains(filter) ||
                                                    d.Status_residence.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Student.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.RoomId == filterNumber ||
                                                    d.GroupId == filterNumber ||
                                                    d.ParentsId == filterNumber));

                return filtered;
            }
        }

        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudent(TextBoxSurName.Text, TextBoxName.Text, TextBoxPatronymic.Text, TextBoxAddress.Text, CheckBoxStatusLearning.IsChecked.Value, ComboBoxFormEducation.Text, ComboBoxStatusResidence.Text, Convert.ToInt32(ComboBoxGroup.Text), Convert.ToInt32(ComboBoxParents.SelectedItem), Convert.ToInt32(ComboBoxRoom.SelectedItem));
            TextBoxSurName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
            TextBoxAddress.Text = null;
            CheckBoxStatusLearning.IsChecked = false;
            ComboBoxRoom.SelectedValue = null;
            ComboBoxStatusResidence.SelectedValue = null;
            ComboBoxFormEducation.SelectedValue = null;
            ComboBoxGroup.SelectedValue = null;
            ComboBoxParents.SelectedValue = null;
            SnackBar("Студент добавлен");
            UpdateData();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSurName.Text = null;
            TextBoxName.Text = null;
            TextBoxPatronymic.Text = null;
            TextBoxAddress.Text = null;
            CheckBoxStatusLearning.IsChecked = false;
            ComboBoxRoom.SelectedValue = null;
            ComboBoxStatusResidence.SelectedValue = null;
            ComboBoxFormEducation.SelectedValue = null;
            ComboBoxGroup.SelectedValue = null;
            ComboBoxParents.SelectedValue = null;
            ButtonUpdateStudent.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAddStudent.IsEnabled = true;
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    Room room = context.Room.FirstOrDefault(r => r.Id == Convert.ToInt32(ComboBoxRoom.Text));
                    Group group = context.Group.FirstOrDefault(g => g.Id == Convert.ToInt32(ComboBoxGroup.Text));
                    Parents parents = context.Parents.FirstOrDefault(p => p.Id == Convert.ToInt32(ComboBoxParents.Text));
                    if (room == null || group == null || parents == null)
                    {
                        SnackBar("Неверные данные");
                        return;
                    }
                    selectedItem.Surname = TextBoxSurName.Text;
                    selectedItem.Name = TextBoxName.Text;
                    selectedItem.Patronymic = TextBoxPatronymic.Text;
                    selectedItem.Home_Address = TextBoxAddress.Text;
                    selectedItem.Status_learning = CheckBoxStatusLearning.IsChecked.Value;
                    selectedItem.RoomId = Convert.ToInt32(ComboBoxRoom.Text);
                    selectedItem.Room = room;
                    selectedItem.Status_residence = ComboBoxStatusResidence.Text;
                    selectedItem.Form_of_education = ComboBoxFormEducation.Text;
                    selectedItem.GroupId = Convert.ToInt32(ComboBoxGroup.Text);
                    selectedItem.ParentsId = Convert.ToInt32(ComboBoxParents.Text);
                }
                context.Student.Update(selectedItem);
                context.SaveChanges();
                SnackBar("Обновление данных");
                UpdateData();
                selectedItem = null;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Student.Remove(selectedItem);
                context.SaveChanges();
                TextBoxSurName.Text = null;
                TextBoxName.Text = null;
                TextBoxPatronymic.Text = null;
                TextBoxAddress.Text = null;
                CheckBoxStatusLearning.IsChecked = false;
                ComboBoxRoom.SelectedValue = null;
                ComboBoxStatusResidence.SelectedValue = null;
                ComboBoxFormEducation.SelectedValue = null;
                ComboBoxGroup.SelectedValue = null;
                ComboBoxParents.SelectedValue = null;
                ButtonUpdateStudent.IsEnabled = false;
                ButtonCancel.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
                ButtonAddStudent.IsEnabled = true;
                SnackBar("Запись удалена");
                UpdateData();
            }
        }
    }

}
