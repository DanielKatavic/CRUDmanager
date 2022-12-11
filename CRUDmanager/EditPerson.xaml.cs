using CRUDmanager.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRUDmanager
{
    /// <summary>
    /// Interaction logic for EditPerson.xaml
    /// </summary>
    public partial class EditPerson : FramedPage
    {
        private Student? _student;

        public EditPerson(ViewModels.UniversityViewModel universityViewModel, Student? selectedStudent = null) : base(universityViewModel)
        {
            InitializeComponent();
            _student = selectedStudent ?? new Student(-1, string.Empty, string.Empty);
            DataContext = _student;
        }

        private void BtnGetBack_Click(object sender, RoutedEventArgs e) => Frame?.GoBack();

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!FormIsValid())
            {
                return;
            }
            _student = DataContext as Student;
            if (_student?.Id == -1)
            {
                UniversityViewModel.Students.Add(_student);
            }
            else
            {
                UniversityViewModel.Update(_student!);
            }
        }

        private bool FormIsValid() => spObjectInfo.Children.OfType<TextBox>().Any(tb => !string.IsNullOrWhiteSpace(tb.Text));
    }
}
