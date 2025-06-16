using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class CategoryLoanReportDetailVM : BaseViewModel
    {
        private BCTONGLUOTMUON baocaotongluotmuon;
        public BCTONGLUOTMUON BaoCaoTongLuotMuon
        {
            get => baocaotongluotmuon;
            set
            {
                if (baocaotongluotmuon != value)
                {
                    baocaotongluotmuon = value;
                    OnPropertyChanged(nameof(BaoCaoTongLuotMuon));
                }
            }
        }

        private ObservableCollection<BCLUOTMUONTHEOTHELOAI> _listCategoryDetails;
        public ObservableCollection<BCLUOTMUONTHEOTHELOAI> ListCategoryDetails
        {
            get => _listCategoryDetails;
            set
            {
                if (_listCategoryDetails != value)
                {
                    _listCategoryDetails = value;
                    OnPropertyChanged(nameof(ListCategoryDetails));
                }
            }
        }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadDataCM { get; set; }
        public CategoryLoanReportDetailVM() { }
        public CategoryLoanReportDetailVM(BCTONGLUOTMUON SelectedReport)
        {
            ListCategoryDetails = new ObservableCollection<BCLUOTMUONTHEOTHELOAI>();
            BaoCaoTongLuotMuon = SelectedReport;
            LoadDataCM = new RelayCommand<Window>((p) => SelectedReport != null, async (p) =>
            {
                try
                {
                    var data = await BaoCaoThongKeBLL.Instance.GetBaoCaoTheLoai(SelectedReport.id);
                    ListCategoryDetails = new ObservableCollection<BCLUOTMUONTHEOTHELOAI>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => p != null, (p) =>
            {
                p.Close();
            });
        }
    }
}