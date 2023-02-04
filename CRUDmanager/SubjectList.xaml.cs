using CRUDmanager.Models;
using CRUDmanager.ViewModels;
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

        private void CbSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Subject? selectedSubject = cbSubjects.SelectedItem as Subject;

            if (selectedSubject is null)
            {
                return;
            }

            FillSubjectInfo(selectedSubject);

            SetSubjectInfoVisible();

            lvStudents.ItemsSource = UniversityViewModel.GetStudentsForSubject(selectedSubject.Id);
        }

        private void FillSubjectInfo(Subject? selectedSubject)
        {
            lblSubjectName.Content = selectedSubject?.Name;
            lblSubjectEcts.Content = selectedSubject?.Ects;
            lblSubjectProfessor.Content = selectedSubject?.Professor;
        }

        private void SetSubjectInfoVisible()
        {
            lblName.Visibility = Visibility.Visible;
            lblEcts.Visibility = Visibility.Visible;
            lblProfessor.Visibility = Visibility.Visible;
            subjectButtons.Visibility = Visibility.Visible;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(new EditPerson(UniversityViewModel) { Frame = Frame });
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvStudents.SelectedItem is not Student selectedStudent)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?") == MessageBoxResult.OK)
            {
                UniversityViewModel.Persons.Remove(selectedStudent);
            }
        }

        private void LvStudents_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvStudents.SelectedItem is Student selectedStudent)
            {
                Frame?.Navigate(new EditPerson(UniversityViewModel, selectedStudent) { Frame = Frame });
            }
        }

        private void BtnEditSubject_Click(object sender, RoutedEventArgs e)
        {
            if (cbSubjects.SelectedItem is Subject selectedSubject)
            {
                Frame?.Navigate(new EditSubject(UniversityViewModel, selectedSubject) { Frame = Frame });
            }
        }

        private void BtnRemoveSubject_Click(object sender, RoutedEventArgs e)
        {
            if (cbSubjects.SelectedItem is not Subject selectedSubject)
            {
                return;
            }
            if (MessageBox.Show($"Are you sure you want to delete {selectedSubject.Name}?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                UniversityViewModel.Subjects.Remove(selectedSubject);
            }
        }
    }
}
