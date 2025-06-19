using System.Windows;

namespace SchoolSystem.View
{
    public partial class AddRemarkWindow : Window
    {
        public AddRemarkWindow()
        {
            InitializeComponent();
        }

        public AddRemarkWindow(ViewModel.AddRemarkViewModel vm) : this()
        {
            DataContext = vm;
        }
    }
}
