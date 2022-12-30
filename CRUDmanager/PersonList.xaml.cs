using CRUDmanager.ViewModels;

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
            lvStudents.ItemsSource = universityViewModel.Students;
            lvProfessors.ItemsSource = universityViewModel.Professors;
        }
    }
}
