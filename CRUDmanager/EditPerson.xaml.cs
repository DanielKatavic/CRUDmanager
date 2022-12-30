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
        public EditPerson(ViewModels.UniversityViewModel universityViewModel, Person? selectedPerson = null) : base(universityViewModel)
        {
            InitializeComponent();
            DataContext = selectedPerson ?? new Student(-1, string.Empty, string.Empty);
        }

        private void BtnGetBack_Click(object sender, RoutedEventArgs e) => Frame?.GoBack();

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!FormIsValid())
            {
                return;
            }
            Person? person = DataContext as Person;
            if (person?.Id == -1)
            {
                if (person is Student)
                {
                    UniversityViewModel.Students.Add(person as Student);
                }
                else
                {
                    UniversityViewModel.Professors.Add(person as Professor);
                }
            }
            else
            {
                UniversityViewModel.Update(person!);
            }
            Frame?.GoBack();
        }

        private bool FormIsValid() => spObjectInfo.Children.OfType<TextBox>().Any(tb => !string.IsNullOrWhiteSpace(tb.Text));
    }
}
