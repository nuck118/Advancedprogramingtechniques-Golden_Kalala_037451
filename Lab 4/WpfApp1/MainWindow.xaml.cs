using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    // Models
    public class Student : INotifyPropertyChanged
    {
        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses
        {
            get => _courses ?? (_courses = new ObservableCollection<Course>());
            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string ToString() => $"{FirstName} {LastName}";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Teacher
    {
        public string Name { get; set; }
        public Student AssignedStudent { get; set; }
        public Course AssignedCourse { get; set; }
        public override string ToString() => $"{Name} (Student: {AssignedStudent?.ToString() ?? "None"}, Course: {AssignedCourse?.Name ?? "None"})";
    }

    public class Course
    {
        public string Name { get; set; }
        public override string ToString() => Name;
    }

    // ViewModel
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Teacher> Teachers { get; set; }
        public ObservableCollection<Course> AllCourses { get; set; }

        private string _studentFirstName;
        public string StudentFirstName
        {
            get => _studentFirstName;
            set
            {
                _studentFirstName = value;
                OnPropertyChanged(nameof(StudentFirstName));
            }
        }

        private string _studentLastName;
        public string StudentLastName
        {
            get => _studentLastName;
            set
            {
                _studentLastName = value;
                OnPropertyChanged(nameof(StudentLastName));
            }
        }

        private string _teacherName;
        public string TeacherName
        {
            get => _teacherName;
            set
            {
                _teacherName = value;
                OnPropertyChanged(nameof(TeacherName));
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));
            }
        }

        private Student _selectedStudentForCourses;
        public Student SelectedStudentForCourses
        {
            get => _selectedStudentForCourses;
            set
            {
                _selectedStudentForCourses = value;
                OnPropertyChanged(nameof(SelectedStudentForCourses));
            }
        }

        private string _newCourseName;
        public string NewCourseName
        {
            get => _newCourseName;
            set
            {
                _newCourseName = value;
                OnPropertyChanged(nameof(NewCourseName));
                UpdateAllCourses();
            }
        }

        public ICommand AddStudentCommand { get; }
        public ICommand RemoveStudentCommand { get; }
        public ICommand AddTeacherCommand { get; }
        public ICommand RemoveTeacherCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand RemoveCourseCommand { get; }

        public MainViewModel()
        {
            Students = new ObservableCollection<Student>();
            Teachers = new ObservableCollection<Teacher>();
            AllCourses = new ObservableCollection<Course>();
            AddStudentCommand = new RelayCommand(AddStudent, CanAddStudent);
            RemoveStudentCommand = new RelayCommand(RemoveStudent, CanRemoveStudent);
            AddTeacherCommand = new RelayCommand(AddTeacher, CanAddTeacher);
            RemoveTeacherCommand = new RelayCommand(RemoveTeacher, CanRemoveTeacher);
            AddCourseCommand = new RelayCommand(AddCourse, CanAddCourse);
            RemoveCourseCommand = new RelayCommand(RemoveCourse, CanRemoveCourse);
        }

        private void AddStudent(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(StudentFirstName) && !string.IsNullOrWhiteSpace(StudentLastName))
            {
                Students.Add(new Student { FirstName = StudentFirstName, LastName = StudentLastName });
                StudentFirstName = string.Empty;
                StudentLastName = string.Empty;
            }
        }

        private bool CanAddStudent(object parameter) =>
            !string.IsNullOrWhiteSpace(StudentFirstName) && !string.IsNullOrWhiteSpace(StudentLastName);

        private void RemoveStudent(object parameter)
        {
            if (parameter is Student student)
            {
                Students.Remove(student);
            }
        }

        private bool CanRemoveStudent(object parameter) => parameter is Student;

        private void AddTeacher(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(TeacherName))
            {
                Teachers.Add(new Teacher { Name = TeacherName, AssignedStudent = SelectedStudent, AssignedCourse = SelectedCourse });
                TeacherName = string.Empty;
                SelectedStudent = null;
                SelectedCourse = null;
            }
        }

        private bool CanAddTeacher(object parameter) =>
            !string.IsNullOrWhiteSpace(TeacherName);

        private void RemoveTeacher(object parameter)
        {
            if (parameter is Teacher teacher)
            {
                Teachers.Remove(teacher);
            }
        }

        private bool CanRemoveTeacher(object parameter) => parameter is Teacher;

        private void AddCourse(object parameter)
        {
            if (SelectedStudentForCourses != null && !string.IsNullOrWhiteSpace(NewCourseName))
            {
                var course = new Course { Name = NewCourseName };
                SelectedStudentForCourses.Courses.Add(course);
                if (!AllCourses.Any(c => c.Name == course.Name))
                {
                    AllCourses.Add(course);
                }
                NewCourseName = string.Empty;
            }
        }

        private bool CanAddCourse(object parameter) =>
            SelectedStudentForCourses != null && !string.IsNullOrWhiteSpace(NewCourseName);

        private void RemoveCourse(object parameter)
        {
            if (parameter is Course course && SelectedStudentForCourses != null)
            {
                SelectedStudentForCourses.Courses.Remove(course);
                UpdateAllCourses();
            }
        }

        private bool CanRemoveCourse(object parameter) => parameter is Course && SelectedStudentForCourses != null;

        private void UpdateAllCourses()
        {
            var uniqueCourses = Students.SelectMany(s => s.Courses)
                                       .GroupBy(c => c.Name)
                                       .Select(g => g.First())
                                       .ToList();
            AllCourses.Clear();
            foreach (var course in uniqueCourses)
            {
                AllCourses.Add(course);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // RelayCommand for ICommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    // MainWindow code-behind
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}