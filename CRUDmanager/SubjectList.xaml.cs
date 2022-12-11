using CRUDmanager.Models;
using CRUDmanager.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CRUDmanager
{
    /// <summary>
    /// Interaction logic for SubjectList.xaml
    /// </summary>
    public partial class SubjectList : FramedPage
    {
        public SubjectList(UniversityViewModel universityViewModel) : base(universityViewModel)
        {
            InitializeComponent();
            cbSubjects.ItemsSource = universityViewModel.Subjects;
        }

        private void cbSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Subject selectedSubject = (Subject)cbSubjects.SelectedItem;
            lblSubjectName.Content = selectedSubject.Name;
            lblSubjectEcts.Content = selectedSubject.Ects;
            lblSubjectProfessor.Content = selectedSubject.Professor;
            lblName.Visibility = Visibility.Visible;
            lblEcts.Visibility = Visibility.Visible;
            lblProfessor.Visibility = Visibility.Visible;

            lvStudents.ItemsSource = UniversityViewModel.GetStudentsForSubject(selectedSubject.Id);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(new EditPerson(UniversityViewModel) { Frame = Frame });
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvStudents?.SelectedItem is null) return;
            if (MessageBox.Show("Are you sure?") == MessageBoxResult.OK)
            {
                UniversityViewModel.Students.Remove((Student)lvStudents.SelectedItem);
            }
        }

        private void lvStudents_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvStudents.SelectedItem is Student selectedStudent)
            {
                Frame?.Navigate(new EditPerson(UniversityViewModel, selectedStudent) { Frame = Frame });
            }
        }
    }
}
