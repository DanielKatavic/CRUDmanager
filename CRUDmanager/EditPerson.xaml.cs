using CRUDmanager.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            DataContext = selectedPerson ?? new Person(-1, string.Empty, string.Empty);
        }

        private void BtnGetBack_Click(object sender, RoutedEventArgs e) => Frame?.GoBack();

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!FormIsValid())
            {
                return;
            }
            var person = DataContext as Person;
            if (person?.Id == -1)
            {
                UniversityViewModel.Persons.Add(person);
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
