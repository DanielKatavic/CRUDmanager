using CRUDmanager.Models;
using CRUDmanager.ViewModels;
using System.Linq;
using System.Windows.Controls;

namespace CRUDmanager
{
    /// <summary>
    /// Interaction logic for PersonList.xaml
    /// </summary>
    public partial class PersonList : FramedPage
    {
        public PersonList(UniversityViewModel universityViewModel) : base(universityViewModel)
        {
            InitializeComponent();
            lvStudents.ItemsSource = universityViewModel.Persons.OfType<Student>();
            lvProfessors.ItemsSource = universityViewModel.Persons.OfType<Professor>();
        }

        private void PersonList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Person? selectedPerson = (sender as ListView)?.SelectedItem as Person;
            if (selectedPerson is null)
            {
                return;
            }
            Frame?.Navigate(new EditPerson(UniversityViewModel, selectedPerson) { Frame = Frame });
        }
    }
}
