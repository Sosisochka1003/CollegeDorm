﻿using MaterialDesignThemes.Wpf;
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
                    combobox.Items.Add(group.Number);
                    ComboBoxGroup.Items.Add(group.Number);
                }
                var Id_room = context.Room.ToList();
                foreach (var room in Id_room)
                {
                    ComboBoxRoom.Items.Add(room.RoomNumber);
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
            await Task.Delay(4000);
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.IsActive = false;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SnackbarOne.IsActive = true;
            SnackBar("Загрузка данных");
            using (var context = new DormContext())
            {
                var stud = context.Student.ToList();
                TestViewStudents.ItemsSource = stud;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.SelectAll();
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
                ComboBoxGroup.SelectedValue = selectedItem.GroupNum;
                ComboBoxParents.SelectedValue = selectedItem.ParentsId;
                ButtonUpdateStudent.Visibility = Visibility.Visible;
                ButtonCancel.Visibility = Visibility.Visible;
                ButtonDelete.Visibility = Visibility.Visible;
            }
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
                var klem = context.Student.Where(s => s.GroupNum.Contains(filterText)).ToList();
                FilteredItems.Clear();
                TestViewStudents.ItemsSource = klem;
            }
        }

        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudent(TextBoxSurName.Text, TextBoxName.Text, TextBoxPatronymic.Text, TextBoxAddress.Text, CheckBoxStatusLearning.IsChecked.Value, ComboBoxFormEducation.Text, ComboBoxStatusResidence.Text, ComboBoxGroup.Text, Convert.ToInt32(ComboBoxParents.SelectedItem), Convert.ToInt32(ComboBoxRoom.SelectedItem));
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
            ButtonUpdateStudent.Visibility = Visibility.Hidden;
            ButtonCancel.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
        }

        private void ButtonUpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    Room room = context.Room.FirstOrDefault(r => r.RoomNumber == Convert.ToInt32(ComboBoxRoom.Text));
                    Group group = context.Group.FirstOrDefault(g => g.Number == ComboBoxGroup.Text);
                    Parents parents = context.Parents.FirstOrDefault(p => p.Id == Convert.ToInt32(ComboBoxParents.Text));
                    if (room == null || group == null || parents == null)
                    {
                        MessageBox.Show("Неверные данные");
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
                    selectedItem.GroupNum = ComboBoxGroup.Text;
                    selectedItem.ParentsId = Convert.ToInt32(ComboBoxParents.Text);
                }
                context.Student.Update(selectedItem);
                context.SaveChanges();
                MessageBox.Show("Обновление данных");

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
                ButtonUpdateStudent.Visibility = Visibility.Hidden;
                ButtonCancel.Visibility = Visibility.Hidden;
                ButtonDelete.Visibility = Visibility.Hidden;
                MessageBox.Show("Запись удалена");
            }
        }
    }

}
