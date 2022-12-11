using CRUDmanager.ViewModels;
using System.Windows.Controls;

namespace CRUDmanager
{
    public class FramedPage : Page
    {
        public UniversityViewModel UniversityViewModel { get; }
        public Frame? Frame { get; set; }

        public FramedPage(UniversityViewModel universityViewModel)
        {
            UniversityViewModel = universityViewModel;
        }
    }
}
