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

namespace SchoolSystem.View.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : UserControl
    {
        public SchedulePage()
        {
            InitializeComponent();
            this.DataContext = new ScheduleViewModel();
        }
    }
}
