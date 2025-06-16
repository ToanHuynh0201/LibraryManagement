using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Borrow;
using LibraryManagement.View.Receipt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class PenaltyRecepitManagementVM : BaseViewModel
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

        private DateTime? _SelectedDate;
        public DateTime? SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                _SelectedDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedDateString));
            }
        }

        public string SelectedDateString
        {
            get
            {
                return SelectedDate?.ToString("dd/MM/yyyy") ?? "";
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
        private PHIEUTHUTIENPHAT _phieuthu { get; set; }
        public PHIEUTHUTIENPHAT phieuthu
        {
            get
            {
                return _phieuthu;
            }
            set
            {
                _phieuthu = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<PHIEUTHUTIENPHAT> _ListPenaltyReceipt;
        public ObservableCollection<PHIEUTHUTIENPHAT> ListPenaltyReceipt
        {
            get
            {
                return _ListPenaltyReceipt;
            }
            set
            {
                _ListPenaltyReceipt = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PHIEUTHUTIENPHAT> AllPenaltyReceipt { get; set; }
        public PHIEUTHUTIENPHAT PenaltyReceiptSelected { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataPenaltyReceiptCM { get; set; }
        public ICommand SearchPenaltyReceiptCM { get; set; }
        public ICommand FilterByDateCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddPenaltyReceiptCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public PenaltyRecepitManagementVM()
        {
            SearchList = new ObservableCollection<string> { "Số phiếu thu", "Mã độc giả" };
            SearchProperties = SearchList.FirstOrDefault();
            LoadDataPenaltyReceiptCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await PhieuThuTienPhatBLL.Instance.GetAllPhieuPhat());
                    ListPenaltyReceipt = new ObservableCollection<PHIEUTHUTIENPHAT>(data);
                    AllPenaltyReceipt = new ObservableCollection<PHIEUTHUTIENPHAT>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchPenaltyReceiptCM = new RelayCommand<object>((p) => true, async (p) =>
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
            FilterByDateCM = new RelayCommand<object>((p) => true, (p) =>
            {
                try
                {
                    ApplyFilterAndSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lọc theo ngày: " + ex.Message);
                }
            });
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                SelectedDate = null;
                ListPenaltyReceipt = new ObservableCollection<PHIEUTHUTIENPHAT>(AllPenaltyReceipt);
            });
            OpenAddPenaltyReceiptCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddPenaltyRecepit();
                vm1.OnSuccess = () => LoadDataPenaltyReceiptCM.Execute(null);
                var w1 = new AddPenaltyReceiptWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }

        private void ApplyFilterAndSearch()
        {
            var result = AllPenaltyReceipt.AsEnumerable();

            if (SelectedDate.HasValue)
            {
                var selectedDateOnly = SelectedDate.Value.Date;
                result = result.Where(pt => pt.NgayThu.Date == selectedDateOnly);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                if (SearchProperties == SearchList[0])
                    result = result.Where(pt => pt.SoPhieuThu.Contains(SearchText) || pt.DOCGIA.MaDG.Contains(SearchText));
            }

            ListPenaltyReceipt = new ObservableCollection<PHIEUTHUTIENPHAT>(result);
        }
    }
}