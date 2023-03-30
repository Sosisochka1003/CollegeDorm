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

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для AllDocuments.xaml
    /// </summary>
    public partial class AllDocuments : Window
    {
        Document selectedItem = new Document();
        public AllDocuments()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var stud = context.Student.ToList();
                TestView.ItemsSource = stud;
            }
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
                var dorm = context.Dormitory.ToList();
                TestView.ItemsSource = dorm;
            }
        }
        public ObservableCollection<Document> FilteredItems { get; set; } = new ObservableCollection<Document>();

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                Student stud = context.Student.FirstOrDefault(s => s.Id == Convert.ToInt32(ComboBoxStudent.SelectedValue));
                if (stud == null)
                {
                    SnackBar("Неверный студент");
                    return;
                }
                Document doc = new Document
                {
                    Name = TextBoxName.Text,
                    StudentId = stud.Id,
                    StudentSurname = stud.Surname,
                    Student = stud
                };
                context.Document.Add(doc);
                context.SaveChanges();
                SnackBar("Добавлена новая запись");
                UpdateData();
                TextBoxName.Text = null;
                ComboBoxStudent.Text = null;
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    Student stud = context.Student.FirstOrDefault(s => s.Id == Convert.ToInt32(ComboBoxStudent.SelectedValue));
                    if (stud == null)
                    {
                        SnackBar("Неверный студент");
                        return;
                    }
                    selectedItem.Name = TextBoxName.Text;
                    selectedItem.StudentId = stud.Id;
                    selectedItem.StudentSurname = stud.Surname;
                    selectedItem.Student = stud;
                }
                context.Document.Update(selectedItem);
                context.SaveChanges();
                SnackBar("Обновление данных");
                ButtonsVisible();
                TextBoxName.Text = null;
                ComboBoxStudent.Text = null;
                UpdateData();
                selectedItem = null;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = null;
            ComboBoxStudent.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.Document.Remove(selectedItem);
                context.SaveChanges();
                TextBoxName.Text = null;
                ComboBoxStudent.Text = null;
                ButtonsVisible();
                SnackBar("Запись удалена");
                UpdateData();
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            SnackBar("Данные из БД");
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = ((TextBox)sender).Text.ToLower();
            using (var context = new DormContext())
            {
                if (filterText != null)
                {
                    var klem = context.Document.Where(d =>
                                                    d.Name.ToLower().Contains(filterText) ||
                                                    d.StudentSurname.ToLower().Contains(filterText))
                                                .ToList();
                    FilteredItems.Clear();
                    TestView.ItemsSource = klem;
                }
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (Document)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxName.Text = selectedItem.Name;
                    ComboBoxStudent.Text = selectedItem.StudentId.ToString();
                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonDelete.Visibility = Visibility.Visible;
                    ButtonAdd.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
