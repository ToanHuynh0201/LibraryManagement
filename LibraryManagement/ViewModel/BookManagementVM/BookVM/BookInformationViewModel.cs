using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace LibraryManagement.ViewModel
{
    public class BookInformationViewModel : BaseViewModel
    {
        public SACH sach { get; set; } = new SACH();
        public ICommand CloseWindowCM { get; set; }
        public BookInformationViewModel() { }
        public BookInformationViewModel(SACH bookSelected)
        {
            sach = bookSelected;
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
    }
}