using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Borrow;
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
    public class BorrowSlipViewModel : BaseViewModel
    {
        #region Properties
        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                _SearchText = value;
                OnPropertyChanged();
            }
        }
        private string _SearchProperties;
        public string SearchProperties
        {
            get
            {
                return _SearchProperties;
            }
            set
            {
                _SearchProperties = value;
                OnPropertyChanged();
            }
        }
        private string _StatusProperties;
        public string StatusProperties
        {
            get
            {
                return _StatusProperties;
            }
            set
            {
                _StatusProperties = value;
                OnPropertyChanged();
            }
        }
        private PHIEUMUONTRA _phieumuontra { get; set; }
        public PHIEUMUONTRA phieumuontra
        {
            get
            {
                return _phieumuontra;
            }
            set
            {
                _phieumuontra = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        public ObservableCollection<string> StatusList { get; set; }
        private ObservableCollection<PHIEUMUONTRA> _ListBorrowSlips;
        public ObservableCollection<PHIEUMUONTRA> ListBorrowSlips
        {
            get
            {
                return _ListBorrowSlips;
            }
            set
            {
                _ListBorrowSlips = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PHIEUMUONTRA> AllBorrowSlips { get; set; }
        public PHIEUMUONTRA BorrowSlipSelected { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataBorrowSlipCM { get; set; }
        public ICommand SearchBorrowSlipCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddBorrowSlipCM { get; set; }
        public ICommand UpdateBorrowSlipCM { get; set; }
        public ICommand ViewBorrowSlipCM { get; set; }
        public ICommand FilterStatusCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BorrowSlipViewModel()
        {
            SearchList = new ObservableCollection<string> { "Số phiếu mượn", "Mã cuốn sách", "Tên đầu sách", "Mã độc giả" };
            SearchProperties = SearchList.FirstOrDefault();
            StatusList = new ObservableCollection<string> { "Tất cả", "Đã trả", "Chưa trả" };
            StatusProperties = StatusList.FirstOrDefault();
            LoadDataBorrowSlipCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await PhieuMuonTraBLL.Instance.GetAllPMT());
                    ListBorrowSlips = new ObservableCollection<PHIEUMUONTRA>(data);
                    AllBorrowSlips = new ObservableCollection<PHIEUMUONTRA>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchBorrowSlipCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    ApplyFilterAndSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            FilterStatusCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    ApplyFilterAndSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lọc trạng thái: " + ex.Message);
                }
            });

            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                StatusProperties = StatusList[0];
                ListBorrowSlips = new ObservableCollection<PHIEUMUONTRA>(AllBorrowSlips);
            });
            OpenAddBorrowSlipCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddNewBorrowViewModel();
                vm1.OnSuccess = () => LoadDataBorrowSlipCM.Execute(null);
                var w1 = new BorrowFormWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });
            UpdateBorrowSlipCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show(
                                            "Xác nhận trả sách?",
                                            "Trả sách",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
                var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.UpdatePhieuMuonTra(BorrowSlipSelected));
                MessageBox.Show(res.Item2);
                if (res.Item1) LoadDataBorrowSlipCM.Execute(null);

            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        private void ApplyFilterAndSearch()
        {
            var result = AllBorrowSlips.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                if (SearchProperties == SearchList[0])
                    result = result.Where(pmt => pmt.SoPhieu.Contains(SearchText));
                else if (SearchProperties == SearchList[1])
                    result = result.Where(pmt => pmt.CUONSACH.MaCuonSach.Contains(SearchText));
                else if (SearchProperties == SearchList[2])
                    result = result.Where(pmt => pmt.CUONSACH.SACH.DAUSACH.TenDauSach.Contains(SearchText));
                else if (SearchProperties == SearchList[3])
                    result = result.Where(pmt => pmt.DOCGIA.MaDG.Contains(SearchText));
            }
            if (StatusProperties == StatusList[1])
                result = result.Where(pmt => pmt.NgayTra != null);
            else if (StatusProperties == StatusList[2])
                result = result.Where(pmt => pmt.NgayTra == null);

            ListBorrowSlips = new ObservableCollection<PHIEUMUONTRA>(result);
        }

    }
}
