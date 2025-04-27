using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class RuleViewModel : BaseViewModel
    {
        #region Properties
        private object _CurrentRule;
        public object CurrentRule
        {
            get
            {
                return _CurrentRule;
            }
            set
            {
                _CurrentRule = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand EditCM { get; set; }
        public ICommand SaveCM { get; set; }
        #endregion

        public RuleViewModel()
        {
            EditCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });
            SaveCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });
        }
    }
}
