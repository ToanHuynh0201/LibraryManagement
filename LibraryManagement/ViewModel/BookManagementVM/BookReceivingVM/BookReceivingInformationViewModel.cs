using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.BookReceiving;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookReceivingInformationViewModel : BaseViewModel
    {
        #region Properties
        public PHIEUNHAPSACH phieunhapsach { get; set; }
        public ObservableCollection<CT_PHIEUNHAPSACH> ctpnsList { get; set; }
        #endregion

        #region Commands
        public ICommand LoadDataCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BookReceivingInformationViewModel() { }
        public BookReceivingInformationViewModel(PHIEUNHAPSACH bookReceivingForm)
        {
            ctpnsList = new ObservableCollection<CT_PHIEUNHAPSACH>();
            phieunhapsach = new PHIEUNHAPSACH();
            phieunhapsach = bookReceivingForm;
            LoadDataCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var res = await CT_PhieuNhapSachBLL.Instance.GetCT_PNSByPhieuNhapSach(phieunhapsach);
                ctpnsList = new ObservableCollection<CT_PHIEUNHAPSACH>(res);
                OnPropertyChanged(nameof(ctpnsList));
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
