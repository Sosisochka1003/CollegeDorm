﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для AllParents.xaml
    /// </summary>
    public partial class AllParents : Window
    {
        Parents selectedItem = new Parents();
        public AllParents()
        {
            InitializeComponent();
        }
        private void ButtonsVisible()
        {
            ButtonUpdate.Visibility = Visibility.Hidden;
            ButtonCancel.Visibility = Visibility.Hidden;
            ButtonDelete.Visibility = Visibility.Hidden;
            ButtonAdd.Visibility = Visibility.Visible;
        }
        public async void SnackBar(string text)
        {
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.Message.Content = text;
                SnackbarOne.IsActive = true;
            });
            await Task.Delay(2500);
            this.Dispatcher.Invoke(() =>
            {
                SnackbarOne.IsActive = false;
            });
        }
        private void UpdateData()
        {
            using (var context = new DormContext())
            {
                var par = context.Parents.ToList();
                TestView.ItemsSource = par;
            }
        }
        public ObservableCollection<Parents> FilteredItems { get; set; } = new ObservableCollection<Parents>();
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Parents par = new Parents
                {
                    Mother = TextBoxMother.Text,
                    Father = TextBoxFather.Text,
                    Marriage = CheckBoxStatusMarriage.IsChecked.Value
                };
                context.Parents.Add(par);
                context.SaveChanges();
                SnackBar("Добавлена новая запись");
                UpdateData();
                TextBoxMother.Text = null;
                TextBoxFather.Text = null;
                CheckBoxStatusMarriage.IsChecked = false;
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    selectedItem.Mother = TextBoxMother.Text;
                    selectedItem.Father = TextBoxFather.Text;
                    selectedItem.Marriage = CheckBoxStatusMarriage.IsChecked.Value;
                }
                context.Parents.Update(selectedItem);
                context.SaveChanges();
                SnackBar("Обновление данных");
                ButtonsVisible();
                TextBoxMother.Text = null;
                TextBoxFather.Text = null;
                CheckBoxStatusMarriage.IsChecked = false;
                UpdateData();
                selectedItem = null;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxMother.Text = null;
            TextBoxFather.Text = null;
            CheckBoxStatusMarriage.IsChecked = false;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Parents.Remove(selectedItem);
                context.SaveChanges();
                TextBoxMother.Text = null;
                TextBoxFather.Text = null;
                CheckBoxStatusMarriage.IsChecked = false;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text.ToLower();
            using (var context = new DormContext())
            {
                if (filterText != null)
                {
                    var klem = context.Parents.Where(p =>
                                                    p.Mother.ToLower().Contains(filterText) ||
                                                    p.Father.ToLower().Contains(filterText))
                                                .ToList();
                    FilteredItems.Clear();
                    TestView.ItemsSource = klem;
                }
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Parents)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxMother.Text = selectedItem.Mother;
                    TextBoxFather.Text = selectedItem.Father;
                    CheckBoxStatusMarriage.IsChecked = selectedItem.Marriage;
                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonDelete.Visibility = Visibility.Visible;
                    ButtonAdd.Visibility = Visibility.Hidden;
                }
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            SnackBar("Данные из БД");
        }
    }
}
