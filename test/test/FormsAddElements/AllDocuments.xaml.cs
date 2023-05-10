using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using System.Reflection;

namespace test.FormsAddElements
{
    /// <summary>
    /// Логика взаимодействия для AllDocuments.xaml
    /// </summary>
    public partial class AllDocuments : Window
    {
        Document selectedItem = new Document();
        Type typeDocument = typeof(Document);
        public AllDocuments()
        {
            InitializeComponent();
            using (var context = new DormContext())
            {
                var stud = context.Student.ToList();
                foreach (var item in stud)
                {
                    ComboBoxStudent.Items.Add(item.Id);
                }
            }
        }
        private void ButtonsVisible()
        {
            ButtonUpdate.IsEnabled = false;
            ButtonCancel.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAdd.IsEnabled = true;
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
                var dorm = context.Document.ToList();
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

                try
                {
                    Document doc = new Document
                    {
                        Name = TextChecker.CheckCyrillic(TextBoxName.Text),
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
                catch (Exception)
                {
                    SnackBar("Неверное значение");
                }
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DormContext())
            {
                try
                {
                    if (selectedItem != null)
                    {
                        Student stud = context.Student.FirstOrDefault(s => s.Id == Convert.ToInt32(ComboBoxStudent.SelectedValue));
                        if (stud == null)
                        {
                            SnackBar("Неверный студент");
                            return;
                        }
                        selectedItem.Name = TextChecker.CheckCyrillic(TextBoxName.Text);
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
                catch (Exception asd)
                {
                    SnackBar("Неверные данные");
                }
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
            var filterText = ((TextBox)sender).Text.ToLower();

            var filtered = GetFilteredResults(filterText);

            FilteredItems.Clear();
            TestView.ItemsSource = filtered;
}

        private List<Document> GetFilteredResults(string filter)
        {
            using (var context = new DormContext())
            {
                var filtered = new List<Document>();

                filtered.AddRange(context.Document.Where(d =>
                                                    d.Name.ToLower().Contains(filter) ||
                                                    d.StudentSurname.ToLower().Contains(filter)));

                var isNumber = int.TryParse(filter, out int filterNumber);

                if (!isNumber)
                {
                    return filtered;
                }

                filtered.AddRange(context.Document.Where(d =>
                                                    d.Id == filterNumber ||
                                                    d.StudentId == filterNumber));

                return filtered;
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
                    ButtonUpdate.IsEnabled = true;
                    ButtonCancel.IsEnabled = true;
                    ButtonDelete.IsEnabled = true;
                    ButtonAdd.IsEnabled = false;
                }
            }
        }
    }
}
