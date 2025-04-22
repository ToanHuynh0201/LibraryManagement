using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Commands
        public ICommand PasswordChangedCM { get; set; }
        #endregion
        #region Proporties
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {   
                password = value;
                OnPropertyChanged();
            }
        }

        #endregion
        public LoginViewModel()
        {
            PasswordChangedCM = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                
            });
        }
    }
}
