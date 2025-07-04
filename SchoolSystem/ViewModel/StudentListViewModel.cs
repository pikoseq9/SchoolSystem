﻿using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.ViewModel
{
    public class StudentListViewModel : BaseViewModel
    {

        private readonly StudentRepository _studentrepository;
        private ObservableCollection<Student>? _students;
        private Student _selectedStudent;


        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(); 
            }
        }

        public StudentListViewModel()
        {



            _studentrepository = new StudentRepository();

            try
            {
                Students = _studentrepository.GetAllStudents(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas ładowania uczniów: {ex.Message}");
            }
        }

        

    }
}
