using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.ViewModel.BaseClass
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged(params string[] properties)
        //{
        //    if (PropertyChanged != null)
        //        foreach (var property in properties)
        //            PropertyChanged(this, new PropertyChangedEventArgs(property));
        //}
    

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) //test czy działa 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}
