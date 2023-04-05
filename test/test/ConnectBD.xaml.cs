using Npgsql;
using System.Windows;
namespace test
{
    /// <summary>
    /// Логика взаимодействия для ConnectBD.xaml
    /// </summary>
    public partial class ConnectBD : Window
    {
        
        public ConnectBD()
        {
            InitializeComponent();
            TextBoxHost.Text = Settings1.Default.Host;
            TextBoxPort.Text = $"{Settings1.Default.Port}";
            TextBoxDataBase.Text = Settings1.Default.DataBase;
            TextBoxUserName.Text = Settings1.Default.UserName;
            TextBoxPassword.Text = Settings1.Default.Password;
        }


        private void ButtonSaveConnectBD_Click(object sender, RoutedEventArgs e)
        {

            Settings1.Default.Host = TextBoxHost.Text;

            if (!int.TryParse(TextBoxPort.Text, out int port))
            {
                MessageBox.Show("Не верно написан порт");
                return;
            }
            Settings1.Default.Port = port;
            Settings1.Default.DataBase = TextBoxDataBase.Text;
            Settings1.Default.UserName = TextBoxUserName.Text;
            Settings1.Default.Password = TextBoxPassword.Text;
            Settings1.Default.Save();
            if (CheckConnection())
            {

                MessageBox.Show("Подключение установлено\nПриложение перезапустить");
                Settings1.Default.IsConnect = true;
                Settings1.Default.Save();
                this.Close();
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Подключения нету");
                Settings1.Default.IsConnect = false;
                Settings1.Default.Save();
            }
        }
        public static bool CheckConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection($"Host={test.Settings1.Default.Host};Port={test.Settings1.Default.Port};Database={test.Settings1.Default.DataBase};Username={test.Settings1.Default.UserName};Password={test.Settings1.Default.Password}");
            try
            {
                conn.Open();
            } catch (Npgsql.NpgsqlException ex) 
            {
                return false; 
            }
            return true;
        }
    }
}
