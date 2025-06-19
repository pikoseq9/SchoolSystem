﻿using SchoolSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSystem.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace SchoolSystem.View.Dashboards
{
    /// <summary>
    /// Logika interakcji dla klasy PrincipalDashboardView.xaml
    /// </summary>
    public partial class PrincipalDashboardView : UserControl
    {

        public PrincipalDashboardView()
        {
            InitializeComponent();
            this.DataContext = new PrincipalDashboardViewModel();
        }
    }
}
