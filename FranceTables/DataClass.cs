using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FranceTables
{
    public class DataClass : INotifyPropertyChanged
    {


        public DataClass()
        {
            IsLoaded = false;
            IsTestsLoaded = false;
        }

        public ObservableCollection<Fact> Facts { get; set; } = new ObservableCollection<Fact>();

        public ObservableCollection<MyTable> Tables { get; set; } = new ObservableCollection<MyTable>();

        public ObservableCollection<MyTest> Tests { get; set; } = new ObservableCollection<MyTest>();

        MyTest currentTest;
        public MyTest CurrentTest
        {
            get { return currentTest; }
            set
            {
                currentTest = value;
                OnPropertyChanged();
            }
        }

        bool isFirst;
        public bool IsFirst
        {
            get { return isFirst; }
            set { isFirst = value; OnPropertyChanged(); }
        }

        bool isLast;
        public bool IsLast
        {
            get { return isLast; }
            set { isLast = value; OnPropertyChanged(); }
        }

        MyTable currentTable;
        public MyTable CurrentTable
        {
            get { return currentTable; }
            set
            {
                currentTable = value;
                if (currentTable == Tables.First())
                    IsFirst = true;
                else
                    IsFirst = false;
                if (currentTable == Tables.Last())
                    IsLast = true;
                else
                    IsLast = false;
                OnPropertyChanged();
            }
        }

        bool isLoaded;
        public bool IsLoaded {
            get { return isLoaded; }
            set {
                isLoaded = value;
                OnPropertyChanged();
            }
        }

        bool isTestsLoaded;
        public bool IsTestsLoaded
        {
            get { return isTestsLoaded; }
            set
            {
                isTestsLoaded = value;
                OnPropertyChanged();
            }
        }

        public MyTable GetTableByFilePath(string Path)
        {
            foreach (var item in Tables)
            {
                if (item.FileName == Path)
                    return item;
            }
            return null;
        }

        public MyTable GetTableByRelativeFilePath(string Path)
        {
            foreach (var item in Tables)
            {
                if (item.FileName.Split('\\').Last() == Path)
                    return item;
            }
            return null;
        }

        public MyTest GetTestByRelativeFilePath(string Path)
        {
            foreach (var item in Tests)
            {
                if (item.FileName.Split('\\').Last() == Path)
                    return item;
            }
            return null;
        }

        public MyTest GetTestByFilePath(string Path)
        {
            foreach (var item in Tests)
            {
                if (item.FileName == Path)
                    return item;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
