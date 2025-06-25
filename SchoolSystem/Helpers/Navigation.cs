using SchoolSystem.ViewModel;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Helpers
{
    public class Navigation
    {

        private Action<BaseViewModel> _navigate;

        public void SetNavigator(Action<BaseViewModel> navigator)
        {
            _navigate = navigator;
        }

        public void NavigateTo(BaseViewModel viewModel)
        {
            _navigate?.Invoke(viewModel);
        }
    }
}
