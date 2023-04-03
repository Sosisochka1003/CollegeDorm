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
    /// Логика взаимодействия для AllStudentSoftStock.xaml
    /// </summary>
    public partial class AllStudentSoftStock : Window
    {
        StudentSoftStock selectedItem = new StudentSoftStock();
        public AllStudentSoftStock()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var stud = context.Student.ToList();
                foreach (var st in stud)
                {
                    ComboBoxStudent.Items.Add(st.Id);
                }
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
                var soft = context.StudentSoftStock.ToList();
                TestView.ItemsSource = soft;
            }
        }
        public ObservableCollection<StudentSoftStock> FilteredItems { get; set; } = new ObservableCollection<StudentSoftStock>();
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
                StudentSoftStock soft = new StudentSoftStock
                {
                    Name = TextBoxName.Text,
                    StudentId = stud.Id,
                    StudentName = stud.Surname,
                    student = stud,
                    Date_issue = DateOnly.FromDateTime((DateTime)DatePickerDateIssue.SelectedDate),
                };
                context.StudentSoftStock.Add(soft);
                context.SaveChanges();
                SnackBar("Добавлена новая запись");
                UpdateData();
                TextBoxName.Text = null;
                ComboBoxStudent.Text = null;
                DatePickerDateIssue.Text = null;
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
                    selectedItem.StudentName = stud.Surname;
                    selectedItem.student = stud;
                    selectedItem.Date_issue = DateOnly.FromDateTime((DateTime)DatePickerDateIssue.SelectedDate);
                }
                context.StudentSoftStock.Update(selectedItem);
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
            DatePickerDateIssue.Text = null;
            ButtonsVisible();
            SnackBar("Операция отменена");
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                context.StudentSoftStock.Remove(selectedItem);
                context.SaveChanges();
                TextBoxName.Text = null;
                ComboBoxStudent.Text = null;
                DatePickerDateIssue.Text= null;
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
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
        }

        private List<StudentSoftStock> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<StudentSoftStock>();

                filtered.AddRange(context.StudentSoftStock.Where(d =>
                                                    d.Name.ToLower().Contains(filter) ||
                                                    d.StudentName.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.StudentSoftStock.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.StudentId == filterNumber));

                return filtered;
            }
        }

        private void TestView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (StudentSoftStock)TestView.SelectedItem;
            using (var context = new DormContext())
            {
                if (selectedItem != null)
                {
                    TextBoxName.Text = selectedItem.Name;
                    ComboBoxStudent.Text = selectedItem.StudentId.ToString();
                    DatePickerDateIssue.Text = selectedItem.Date_issue.ToString();
                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonDelete.Visibility = Visibility.Visible;
                    ButtonAdd.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
