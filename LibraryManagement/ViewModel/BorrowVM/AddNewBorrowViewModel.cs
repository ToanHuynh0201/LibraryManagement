using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AddNewBorrowViewModel : BaseViewModel
    {
        public PHIEUMUONTRA phieumt { get; set; } = new PHIEUMUONTRA();
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
        private string _bookcopyText { get; set; }
        public string BookCopyText
        {
            get => _bookcopyText;
            set
            {
                if (_bookcopyText != value)
                {
                    _bookcopyText = value;
                    OnPropertyChanged(nameof(BookCopyText));
                    if (_isSearching) _ = BookCopySearch();
                }
            }
        }
        public ObservableCollection<DOCGIA> AllReader { get; set; }
        public ObservableCollection<CUONSACH> AllBookCopy { get; set; }
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
        private ObservableCollection<CUONSACH> _listBookCopy { get; set; }
        public ObservableCollection<CUONSACH> ListBookCopy
        {
            get => _listBookCopy;
            set
            {
                if (_listBookCopy != value)
                {
                    _listBookCopy = value;
                    OnPropertyChanged(nameof(ListBookCopy));
                }
            }
        }
        private DOCGIA _readerSelected;
        public DOCGIA ReaderSelected
        {
            get => _readerSelected;
            set
            {
                _readerSelected = value;
                OnPropertyChanged(nameof(ReaderSelected));
                if (_readerSelected != null)
                {
                    _isSearching = false;
                    ReaderText = _readerSelected.DisplayName;
                    _isSearching = true;
                }
            }
        }
        private CUONSACH _bookcopySelected;
        public CUONSACH BookCopySelected
        {
            get => _bookcopySelected;
            set
            {
                _bookcopySelected = value;
                OnPropertyChanged(nameof(BookCopySelected));
                if (_bookcopySelected != null)
                {
                    _isSearching = false;
                    BookCopyText = _bookcopySelected.DisplayName;
                    _isSearching = true;
                }
            }
        }
        private DateTime ngaymuon;
        public DateTime NgayMuon
        {
            get => ngaymuon;
            set
            {
                if (ngaymuon != value)
                {
                    ngaymuon = value;
                    OnPropertyChanged(nameof(NgayMuon));
                    _ = UpdateHanTra();
                }
            }
        }

        private DateTime hantra;
        public DateTime HanTra
        {
            get => hantra;
            set
            {
                if (hantra != value)
                {
                    hantra = value;

                    OnPropertyChanged(nameof(HanTra));

                    phieumt.HanTra = hantra;
                }
            }
        }
        private bool _isSearching = true;
        public Action OnSuccess { get; set; }

        public ICommand AddNewBorrowCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddNewBorrowViewModel()
        {
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var bookcopyData = await CuonSachBLL.Instance.GetAllCuonSachChuaMuon();
                var readerData = await DocGiaBLL.Instance.GetAllDocGia();
                AllReader = ListReader = new ObservableCollection<DOCGIA>(readerData);
                AllBookCopy = ListBookCopy = new ObservableCollection<CUONSACH>(bookcopyData);

                NgayMuon = DateTime.Now;
            });
            AddNewBorrowCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                if (BookCopySelected == null) phieumt.MaCuonSach = -1;
                else phieumt.MaCuonSach = BookCopySelected.id;
                if (ReaderSelected == null) phieumt.MaDG = -1;
                else phieumt.MaDG = ReaderSelected.id;
                phieumt.NgayMuon = NgayMuon;
                phieumt.HanTra = HanTra;
                var res = await Task.Run(async () => await PhieuMuonTraBLL.Instance.AddPhieuMuonTra(phieumt));
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
        private async Task BookCopySearch()
        {
            var res = await Task.Run(async () => await CuonSachBLL.Instance.GetCuonSachChuaDuocMuonByMaCuonSach(BookCopyText));
            ListBookCopy = new ObservableCollection<CUONSACH>(res);
        }
        private async Task UpdateHanTra()
        {
            var res = await ThamSoBLL.Instance.GetThamSo();
            int songaymuon = res.SoNgayMuonToiDa;

            HanTra = NgayMuon.AddDays(songaymuon);
        }

    }
}