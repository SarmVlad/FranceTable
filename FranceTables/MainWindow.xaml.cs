using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FranceTables
{
    public partial class MainWindow : Window
    {
        public string[] files;
        static Page Table, Test;
        bool isTableShow;
        static Frame frame;

        public static DataClass data = new DataClass();

        public MainWindow()
        {
            InitializeComponent();
            frame = Nav;
            isTableShow = false;
            try
            {
                string allText = File.ReadAllText(System.IO.Path.GetFullPath("Facts.txt"), Encoding.Default);
                foreach (string oneFact in allText.Split(new[] { "&&" }, StringSplitOptions.None))
                {
                    data.Facts.Add(new Fact(oneFact.Split('#')[0].Replace(Environment.NewLine, ""), oneFact.Split('#')[1]));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            #region TablesLoad
            if (Table == null)
                Table = new Table();

            data.Tables.Clear();

            files = Directory.GetFiles(System.IO.Path.GetFullPath("Tables"));

            foreach (var file in files)
            {

                #region Excel

                FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);

                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                DataSet result = excelReader.AsDataSet();
                DataTable dt = result.Tables[0];

                excelReader.Close();
                stream.Close();

                #endregion

                data.Tables.Add(new MyTable(file, dt));
            }

            data.IsLoaded = true;

            data.CurrentTable = data.Tables[0];
            //MessageBox.Show((sender as Button).Name);
            Nav.Navigate(Table);
            (Table as Table).Load();
            //Логика загрузки файлов
            #endregion

            #region TestsLoad
            if (Test == null)
                Test = new Test();

            data.Tests.Clear();

            files = Directory.GetFiles(System.IO.Path.GetFullPath("Tests"));

            foreach (var file in files)
            {

                #region Excel

                FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);

                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                DataSet result = excelReader.AsDataSet();
                DataTable dt = result.Tables[0];

                excelReader.Close();
                stream.Close();

                #endregion

                data.Tests.Add(new MyTest(file, dt));
            }
            

            data.CurrentTest = data.Tests[0];
            #endregion

            DataContext = data;
        }

        static public void StartTest()
        {
            frame.Navigate(Test);
            data.CurrentTest = data.GetTestByRelativeFilePath(data.CurrentTable.Test);
        }


        private void TableChoice_Click(object sender, RoutedEventArgs e)
        {
            //CurrentTable = GetTableByName("(sender as Button).Name");
            Nav.Navigate(Table);
            data.CurrentTable = data.GetTableByFilePath((sender as Button).Tag.ToString());
            (Table as Table).Load();
        }

        private void TestChoice_Click(object sender, RoutedEventArgs e)
        {
            //CurrentTable = GetTableByName("(sender as Button).Name");
            Nav.Navigate(Test);
            data.CurrentTest = data.GetTestByFilePath((sender as Button).Tag.ToString());
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Nav.Navigate(new Uri("Help.xaml", UriKind.Relative));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl && Nav.Content.ToString() == "FranceTables.Test")
            {
                try
                {
                    Nav.Navigate(Table);
                    data.CurrentTable = data.GetTableByRelativeFilePath(data.CurrentTest.TableFileName);
                    (Table as Table).Load();
                    isTableShow = true;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка при переключении на таблицу: " + exc.Message);
                }
            }
            
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl && isTableShow)
            {
                Nav.Navigate(Test);
                isTableShow = false;
            }
        }

        private void Facts_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(data.IsLoaded.ToString());
            Nav.Navigate(new Uri("Facts.xaml", UriKind.Relative));
        }
    }
}
