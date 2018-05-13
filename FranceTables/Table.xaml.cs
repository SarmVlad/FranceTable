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

namespace FranceTables
{
    /// <summary>
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class Table : Page
    {
        public string[] files;
        DataClass data = MainWindow.data;

        public Table()
        {
            InitializeComponent();

            DataContext = data;
        }

        public void Load()
        {
            data.CurrentTable.Load(ref MainGrid);
        }

        private void TitleInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(data.CurrentTable.TitleTip) && e.Key == Key.Enter && !string.IsNullOrEmpty(TitleInput.Text))
                MessageBox.Show("Votre réponse est:\n" + TitleInput.Text + "\nLa bonne réponse est:\n" + data.CurrentTable.RightTitle);
        }


        void StartTest_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.StartTest();
        }

        void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (data.Tables.IndexOf(data.CurrentTable) != 0)
            {
                data.CurrentTable = data.Tables[data.Tables.IndexOf(data.CurrentTable) - 1];
                Load();
            }
        }

        void Next_Click(object sender, RoutedEventArgs e)
        {
            if (data.Tables.IndexOf(data.CurrentTable) != data.Tables.Count - 1)
            {
                data.CurrentTable = data.Tables[data.Tables.IndexOf(data.CurrentTable) + 1];
                Load();
            }
        }

        void Tip_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(data.CurrentTable.TitleTip))
                MessageBox.Show("Votre réponse est:\n" + data.CurrentTable.Title+ "\nLa bonne réponse est:\n" + data.CurrentTable.RightTitle);
        }
    }
}
