using LibraryManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AddAuthorViewModel : BaseViewModel
    {
        #region Properties
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private Window window { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCM { get; set; }
        public ICommand GetThisWindowCM { get; set; }
        public ICommand ExitCM { get; set; }
        public ICommand EditCM { get; set; }
        #endregion

        public AddAuthorViewModel()
        {
            GetThisWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                window = p;
            });
            ExitCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                window.Close();
            });
            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });
        }
    }
}
