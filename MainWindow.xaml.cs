using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.viewModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel();

            var vm = (ViewModel)DataContext;

            StreamReader reader = new StreamReader(@"Preferences\MySQLConnectionData.txt");
            Dictionary<string, string> map = new Dictionary<string, string>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] temp = line.Split(':');
                if (!map.ContainsKey(temp[0]))
                {
                    map.Add(temp[0].ToLower(), temp[1]);
                }
            }
            reader.Close();
            vm.Host = map["host"];
            vm.User = map["user"];
            vm.Password = map["password"];
            vm.Database = map["database"];
            vm.Port = map["port"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModel)DataContext;

            try 
            {
                string connectionString =
                    $"Server={vm.Host};" +
                    $"Port={vm.Port};" +
                    $"Database={vm.Database};" +
                    $"User ID={vm.User};" +
                    $"Password={vm.Password};";

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    vm.Message = "Sikeres csatlakozás!";
                }
            } catch (Exception ex)
            {
                StreamWriter writer = new StreamWriter("Errrorlog.txt", append: true);
                writer.WriteLine(ex);
                writer.Close();
                vm.Message = "Sikertelen csatlakozás.";
            }
        }
    }
}
