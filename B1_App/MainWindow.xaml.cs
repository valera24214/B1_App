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
using System.Windows.Navigation;
using System.Windows.Shapes;
using B1_App.UserControls;

namespace B1_App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void task1_button_Click(object sender, RoutedEventArgs e)
        {
            stackPanel1.Children.Clear();
            UserControl1 userControl1 = new UserControl1();
            stackPanel1.Children.Add(userControl1);
        }

        private void task2_button_Click(object sender, RoutedEventArgs e)
        {
            stackPanel1.Children.Clear();
            UserControl2 userControl2 = new UserControl2();
            stackPanel1.Children.Add(userControl2);
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
