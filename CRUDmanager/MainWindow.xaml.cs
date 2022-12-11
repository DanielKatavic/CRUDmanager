using CRUDmanager.Dal;
using CRUDmanager.Models;
using CRUDmanager.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            MainFrame.Navigate(new SubjectList(new UniversityViewModel()) { Frame = MainFrame });
        }
    }
}
