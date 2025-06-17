using SchoolSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolSystem.View.Dashboards
{
    /// <summary>
    /// Logika interakcji dla klasy StudentDashBoardView.xaml
    /// </summary>
    public partial class StudentDashboardView : UserControl
    {
        public StudentDashboardView(int studentId)
        {
            InitializeComponent();
            this.DataContext = new StudentDashboardViewModel(studentId);
        }
    }
}
