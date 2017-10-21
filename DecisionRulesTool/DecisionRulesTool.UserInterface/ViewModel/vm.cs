using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class Student
    {
        public String StudentName { get; set; }
        public int StudentId { get; set; }

        public List<decimal> ProjectScores
        {
            get
            {
                List<decimal> list = new List<decimal>();
                for (int i = 0; i < 100; i++)
                {
                    list.Add(2000 + i);
                }
                return list;
            }
        }
    }
    public class StudentViewModel : BaseWindowViewModel
    {

        private ObservableCollection<Student> _studentList;
        public ObservableCollection<Student> StudentList
        {
            get
            {
                return _studentList;
            }
            set
            {
                if (_studentList != value)
                {
                    _studentList = value;
                    OnPropertyChanged("StudentList");
                }
            }
        }

        private List<string> _titleList;
        public List<string> TitleList
        {
            get
            {
                return _titleList;
            }
            set
            {
                if (_titleList != value)
                {
                    _titleList = value;
                    OnPropertyChanged("TitleList");
                }
            }
        }

        public StudentViewModel()
        {
            PopulateStudents();
        }


        public void PopulateStudents()
        {
            var itemList = new ObservableCollection<Student>();
            for (int i = 0; i < 100; i++)
            {
                itemList.Add(new Student() { StudentName = "A very, very, very, very, long student Name: " + i });
            }

            StudentList = itemList;

            var itemNameList = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                itemNameList.Add("Header + " + i);
            }
            TitleList = itemNameList;
        }

    }
}
