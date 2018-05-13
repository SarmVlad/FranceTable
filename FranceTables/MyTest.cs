using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FranceTables
{
    public class MyTest
    {
        public MyTest(string file, DataTable data)
        {
            Questions = new ObservableCollection<Question>();
            FileName = file;
            Title = data.Rows[0][0].ToString();
            TableFileName = data.Rows[0][1].ToString();
            for (int i = 1; i < data.Rows.Count; i++)
            {
                Questions.Add(new Question(data.Rows[i][0].ToString(), data.Rows[i][1].ToString()));
            }
        }
        ObservableCollection<Question> q = new ObservableCollection<Question>();
        public ObservableCollection<Question> Questions
        {
            get { return q; }
            set
            {
                q = value;
            }
        }
        

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
        public string TableFileName { get; set; }
        public string Title { get; set; }
        
    }

    public class Question : INotifyPropertyChanged
    {
        public Question(string probl, string answer)
        {
            Problem = probl;
            Request = answer;
            ShowAnswer = false;
        }
        public string Problem { get; set; }
        public string Request { get; set; }
        public string Text { get; set; }

        bool showAnswer;
        public bool ShowAnswer
        {
            get { return showAnswer; }
            set
            {
                showAnswer = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
