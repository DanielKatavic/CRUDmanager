using CRUDmanager.ViewModels;
using System.Windows;

namespace CRUDmanager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SubjectListFrame.Navigate(new SubjectList(new UniversityViewModel()) { Frame = SubjectListFrame });
            PersonListFrame.Navigate(new PersonList(new UniversityViewModel()) { Frame = PersonListFrame });
        }
    }
}
