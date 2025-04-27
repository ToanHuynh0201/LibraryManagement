using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AccountViewModel : BaseViewModel
    {
        #region Properties
        private object _CurrentStaff;
        public object CurrentStaff
        {
            get 
            { 
                return _CurrentStaff; 
            }
            set
            {
                _CurrentStaff = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand UpdateImageCM { get; set; } 
        public ICommand ChangePassCM { get; set; } 
        public ICommand LogoutCM { get; set; } 
        #endregion
        public AccountViewModel()
        {
            UpdateImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ChangePassCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            LogoutCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
