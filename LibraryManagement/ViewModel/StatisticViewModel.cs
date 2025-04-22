using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class StatisticViewModel : BaseViewModel
    {
        #region Properties
        public Frame NavigationFrame { get; set; }
        #endregion
        #region Commands
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand BookStatisticCM { get; set; }
        public ICommand ReaderStatisticCM { get; set; }
        #endregion

        public StatisticViewModel()
        {
            GetNavigationFrameCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            BookStatisticCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
            ReaderStatisticCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

            });
        }
    }
}
