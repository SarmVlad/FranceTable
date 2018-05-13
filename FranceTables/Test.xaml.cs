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

namespace FranceTables
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        public Test()
        {
            InitializeComponent();

            DataContext = MainWindow.data;
        }

        void Answer_Click(object sender, RoutedEventArgs e)
        {
           
            foreach (var item in MainWindow.data.CurrentTest.Questions)
            {
                item.ShowAnswer = true;
            }
            MainWindow.data.CurrentTest.Questions[0].ShowAnswer = true;
        }

        private void ShowTable_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl) ;
                //MessageBox.Show("Adaw");
        }
    }
}
