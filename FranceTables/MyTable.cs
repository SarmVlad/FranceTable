using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FranceTables
{
    public class MyTable
    {

        public MyTable(string fileName, DataTable dataTable)
        {
            if(dataTable.Rows[0][2].ToString() != "")
            {
                TestExist = true;
                Test = dataTable.Rows[0][2].ToString();
            }
            else
                TestExist = false;

            //Неправильно находит кол-во столбцов, определяем сами
            int ColumnCount = 0;
            while (dataTable.Rows[1][ColumnCount].ToString() != "")
                ColumnCount++;
            for (int k = dataTable.Columns.Count-1; k > ColumnCount-1; k--)
                dataTable.Columns.RemoveAt(k);
            //MessageBox.Show(dataTable.Rows.Count.ToString() + ' ' + dataTable.Columns.Count.ToString() + ' ' + ColumnCount);
            //Если есть ответ на заголовок
            if (dataTable.Rows[0][1].ToString() != "")
            {
                IsNamed = false;
                RightTitle = dataTable.Rows[0][1].ToString();
                TitleTip = dataTable.Rows[0][0].ToString();
            }
            else //Иначе заголовок задан сразу
            {
                IsNamed = true;
                RightTitle = "";
                TitleTip = "";
                Title = dataTable.Rows[0][0].ToString();
            }
            //После получения нужных нам данных стираем первую строку чтобы не мешала
            dataTable.Rows.RemoveAt(0);
            int max = dataTable.Rows.Count;
            for (int c = 0; c < max; c++)
            {
                if(dataTable.Rows[c][0].ToString() != "" && dataTable.Rows[c][1].ToString() == "")
                {
                    InfoLines.Add(dataTable.Rows[c][0].ToString());
                    dataTable.Rows.RemoveAt(c);
                    c--;
                    max--;
                }
            }

            //Представим в виде строк содержащих столбцы
            MyRows = new List<MyRow>();
            int i = 0;
            while (i < dataTable.Rows.Count)
            {
                MyRow row = new MyRow();
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row.Cells.Add(new MyCell(dataTable.Rows[i][j].ToString()));
                }
                MyRows.Add(row);
                i++;
            }

            int[] length = new int[dataTable.Columns.Count];
            for (int y = 1; y < dataTable.Rows.Count; y++)
            {
                for (int x = 0; x < dataTable.Columns.Count; x++)
                {
                    if (dataTable.Rows[y][x].ToString() != "" && length[x] < GetText(dataTable.Rows[y][x].ToString()).Length)
                    {
                        length[x] = GetText(dataTable.Rows[y][x].ToString()).Length * 15;
                    }
                }
            }
            #region MakeTable
            LabelTable = new Label[dataTable.Rows.Count, dataTable.Columns.Count];
            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                for (int y = 0; y < dataTable.Columns.Count; y++)
                {
                    LabelTable[x, y] = new Label
                    {
                        Content = GetText(dataTable.Rows[x][y].ToString()),
                        VerticalAlignment = VerticalAlignment.Top,
                        Height = 27
                    };
                    if (GetTip(dataTable.Rows[x][y].ToString()) != "")
                        LabelTable[x, y].ToolTip = GetTip(dataTable.Rows[x][y].ToString());
                    Brush border_brush;
                    Thickness border = LabelTable[x, y].BorderThickness;
                    if (GetColor(dataTable.Rows[x][y].ToString()) == "red")
                    {
                        border_brush = Brushes.Red;
                        border.Bottom = 2;
                        border.Top = 2;
                        border.Left = 2;
                        border.Right = 2;
                    }

                    else
                    {
                        border_brush = Brushes.Black;
                        border.Bottom = 1;
                        border.Top = 1;
                        border.Left = 1;
                        border.Right = 1;
                    }
                    ToolTipService.SetInitialShowDelay(LabelTable[x, y], 100);
                    Thickness margin = LabelTable[x, y].Margin;
                    int l = 0;
                    for (int c = y; c > 0; c--)
                    {
                        l += length[c - 1] * 2 - 1;
                    }
                    margin.Left = l;
                    margin.Top = x * (LabelTable[x, y].Height - 1);//+ Отступ для заголовка/// убрал
                    LabelTable[x, y].Width = length[y] * 2;
                    LabelTable[x, y].HorizontalAlignment = HorizontalAlignment.Left;
                    LabelTable[x, y].Margin = margin;


                    LabelTable[x, y].BorderBrush = border_brush;
                    LabelTable[x, y].BorderThickness = border;
                    if (x == 0)
                        LabelTable[x, y].FontWeight = FontWeights.Bold;
                }
            }
            #endregion

            FileName = fileName;
        }

        public void Load(ref Grid grid)
        {
            grid.Children.Clear();
            foreach (var item in LabelTable)
            {
                if(item != null)
                grid.Children.Add(item);
            }
            //
            //grid.Height = LabelTable[LabelTable.GetLength(0) - 1, 0].Margin.Top + LabelTable[LabelTable.GetLength(0) - 1, 0].Height + 20;
            //grid.Width = LabelTable[0, LabelTable.GetLength(1) - 1].Margin.Left + LabelTable[0, LabelTable.GetLength(1) - 1].Width + 10;
        }

        public Label[,] LabelTable;

        public List<string> InfoLines { get; set; } = new List<string>();

        public List<MyRow> MyRows { get; set; }

        public string TitleTip { get; set; }

        public string RightTitle { get; set; }

        public bool IsNamed { get; set; }

        public bool TestExist { get; set; }

        public string Title { get; set; }

        public string Test { get; set; }
        

        string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                string str = value.Split('\\').Last();
                str = str.Split('.').First();
                str = str.Replace('_', ' ');
                NormalName = str;
            }
        }

        public string NormalName { get; set; }


        string GetText(string s)
        {
            string[] strs = s.Split('%');
            return strs[0];
        }
        string GetTip(string s)
        {
            string[] strs = s.Split('%');
            if (strs.Length > 2)
                return strs[2];
            else if (strs.Length > 1 && strs[1] != "red")
                return strs[1];
            else
                return "";
        }
        string GetColor(string s)
        {
            string[] strs = s.Split('%');
            if (strs.Length > 1)
                return strs[1];
            else
                return "";
        }
    }

    public class MyRow
    {
        public MyRow()
        {
            Cells = new List<MyCell>();
        }
        public List<MyCell> Cells { get; set; }
    }

    public class MyCell
    {
        public MyCell(string text)
        {
            Text = text;
            IsRed = false;
        }
        public MyCell(string text, string Tip)
        {
            Text = text;
            this.Tip = Tip;
            IsRed = false;
        }
        public MyCell(string text, string Tip, bool isred)
        {
            Text = text;
            this.Tip = Tip;
            IsRed = isred;
        }
        public string Text { get; set; }
        public string Tip { get; set; }
        public bool IsRed { get; set; }
    }
}
