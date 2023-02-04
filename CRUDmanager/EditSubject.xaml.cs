using CRUDmanager.Models;
using System.Linq;
using System.Windows.Controls;

namespace CRUDmanager
{
    /// <summary>
    /// Interaction logic for EditSubject.xaml
    /// </summary>
    public partial class EditSubject : FramedPage
    {
        public EditSubject(ViewModels.UniversityViewModel universityViewModel, Subject? selectedSubject = null) : base(universityViewModel)
        {
            InitializeComponent();
            DataContext = selectedSubject ?? new Subject(-1, string.Empty, 0, new Professor(new Person(0, string.Empty, string.Empty)));
        }

        private void BtnConfirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!FormIsValid())
            {
                return;
            }
            var subject = DataContext as Subject;
            if (subject?.Id == -1)
            {
                UniversityViewModel.Subjects.Add(subject);
            }
            else
            {
                UniversityViewModel.UpdateModel(subject!);
            }
            Frame?.GoBack();
        }

        private void BtnGetBack_Click(object sender, System.Windows.RoutedEventArgs e) => Frame?.GoBack();

        private bool FormIsValid() => spObjectInfo.Children.OfType<TextBox>().Any(tb => !string.IsNullOrWhiteSpace(tb.Text));
    }
}
