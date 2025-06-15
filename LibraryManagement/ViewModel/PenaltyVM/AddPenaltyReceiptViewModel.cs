using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

namespace LibraryManagement.ViewModel
{
    public class AddPenaltyRecepit : BaseViewModel
    {
        public PHIEUTHUTIENPHAT phieuthu { get; set; } = new PHIEUTHUTIENPHAT();
        private string _readerText { get; set; }
        public string ReaderText
        {
            get => _readerText;
            set
            {
                if (_readerText != value)
                {
                    _readerText = value;
                    OnPropertyChanged(nameof(ReaderText));
                    if (_isSearching) _ = ReaderSearch();
                }
            }
        }
        public ObservableCollection<DOCGIA> AllReader { get; set; }
        private ObservableCollection<DOCGIA> _listReader { get; set; }
        public ObservableCollection<DOCGIA> ListReader
        {
            get => _listReader;
            set
            {
                if (_listReader != value)
                {
                    _listReader = value;
                    OnPropertyChanged(nameof(ListReader));
                }
            }
        }
        private DOCGIA _readerSelected;
        public DOCGIA ReaderSelected
        {
            get => _readerSelected;
            set
            {
                if (_readerSelected != value)
                {
                    _readerSelected = value;
                    OnPropertyChanged(nameof(ReaderSelected));
                    if (_readerSelected != null)
                    {
                        _isSearching = false;
                        ReaderText = _readerSelected.DisplayName;
                        _isSearching = true;
                        SoTienThu = 0;
                        if (_readerSelected.TongNo != null)
                            TongNo = (int)_readerSelected.TongNo;
                        else TongNo = 0;
                    }
                }
            }
        }
        public int TongNo { get; set; }
        private int sotienthu;
        public int SoTienThu
        {
            get => sotienthu;
            set
            {
                if (sotienthu != value)
                {
                    sotienthu = value;
                    OnPropertyChanged(nameof(SoTienThu));
                    if(ReaderSelected.TongNo != null)
                    TongNoMoi = (int)(ReaderSelected.TongNo - SoTienThu);
                }
            }
        }
        private int tongnomoi;
        public int TongNoMoi
        {
            get => tongnomoi;
            set
            {
                if (TongNoMoi != value)
                {
                    tongnomoi = value;
                    OnPropertyChanged(nameof(TongNoMoi));
                }
            }
        }
        private bool _isSearching = true;
        public Action OnSuccess { get; set; }

        public ICommand AddNewPenaltyReceiptCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddPenaltyRecepit()
        {
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var readerData = await DocGiaBLL.Instance.GetAllDocGia();
                AllReader = ListReader = new ObservableCollection<DOCGIA>(readerData);

                phieuthu.SoTienThu = 0;
                phieuthu.NgayThu = DateTime.Now;
                OnPropertyChanged(nameof(phieuthu));
            });
            AddNewPenaltyReceiptCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                if (ReaderSelected == null) phieuthu.MaDG = -1;
                else phieuthu.MaDG = ReaderSelected.id;
                phieuthu.SoTienThu = SoTienThu;
                var res = await Task.Run(async () => await PhieuThuTienPhatBLL.Instance.AddPhieuPhat(phieuthu));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    OnSuccess?.Invoke();
                    p.Close();
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
        private async Task ReaderSearch()
        {
            var res = await Task.Run(async () => await DocGiaBLL.Instance.GetDocGiaByMaDG(ReaderText));
            ListReader = new ObservableCollection<DOCGIA>(res);
        }
    }
}