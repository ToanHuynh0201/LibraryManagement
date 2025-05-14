using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
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
    public class BorrowReturnSlipViewModel : BaseViewModel
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
        private ObservableCollection<PHIEUMUONTRA> _ListBorrowReturnSlips;
        public ObservableCollection<PHIEUMUONTRA> ListBorrowReturnSlips
        {
            get
            {
                return _ListBorrowReturnSlips;
            }
            set
            {
                _ListBorrowReturnSlips = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PHIEUMUONTRA> AllBorrowReturnSlips { get; set; }
        public PHIEUMUONTRA BorrowReturnSlipSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataBorrowReturnSlipCM { get; set; }
        public ICommand SearchBorrowReturnSlipCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddBorrowReturnSlipCM { get; set; }
        public ICommand OpenUpdateBorrowReturnSlipCM { get; set; }
        public ICommand AddNewBorrowReturnSlipCM { get; set; }
        public ICommand ViewBorrowReturnSlipCM { get; set; }
        public ICommand UpdateBorrowReturnSlipCM { get; set; }
        public ICommand DeleteBorrowReturnSlipCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public BorrowReturnSlipViewModel()
        {
            SearchList = new ObservableCollection<string> { "Tên giấy mượn trả" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataBorrowReturnSlipCM = new RelayCommand<PHIEUMUONTRA>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await PhieuMuonTraBLL.Instance.GetAllPMT());
                    ListBorrowReturnSlips = new ObservableCollection<PHIEUMUONTRA>(data);
                    AllBorrowReturnSlips = new ObservableCollection<PHIEUMUONTRA>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchBorrowReturnSlipCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListBorrowReturnSlips.Clear();
                        ListBorrowReturnSlips = AllBorrowReturnSlips;
                    }
                    else
                    {
                        if (SearchProperties == "Mã đọc giả")
                        {
                            var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.GetPMTByMaDG(SearchText));
                            ListBorrowReturnSlips = new ObservableCollection<PHIEUMUONTRA>(res);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListBorrowReturnSlips = new ObservableCollection<PHIEUMUONTRA>(AllBorrowReturnSlips);
            });
            OpenAddBorrowReturnSlipCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                phieumuontra = new PHIEUMUONTRA();
                var w1 = new AddBorrowReturnSlipWindow();
                w1.ShowDialog();
            });
            OpenUpdateBorrowReturnSlipCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditBorrowReturnSlipInformationWindow();
                w1.ShowDialog();
            });
            AddNewBorrowReturnSlipCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.AddPhieuMuonTra(phieumuontra));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBorrowReturnSlipCM.Execute(null);
                    p.Close();
                }
            });
            ViewBorrowReturnSlipCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new BorrowReturnSlipInformtationWindow();
                w1.ShowDialog();
            });
            UpdateBorrowReturnSlipCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.UpdatePhieuMuonTra(BorrowReturnSlipSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataBorrowReturnSlipCM.Execute(null);
                    p.Close();
                }
            });
            DeleteBorrowReturnSlipCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa phiếu mượn trả này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.DeletePhieuMuonTra(BorrowReturnSlipSeleted.id));
                    LoadDataBorrowReturnSlipCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
