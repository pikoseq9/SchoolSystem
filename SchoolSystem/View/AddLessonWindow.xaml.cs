﻿using System;
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
using System.Windows.Shapes;

namespace SchoolSystem.View
{
    /// <summary>
    /// Logika interakcji dla klasy AddLessonWindow.xaml
    /// </summary>
    public partial class AddLessonWindow : Window
    {
        public AddLessonWindow()
        {
            InitializeComponent();
        }

        public AddLessonWindow(int teacherId, int classId)
        {
            InitializeComponent();
            DataContext = new AddLessonViewModel(teacherId, classId);
        }
    }
}
