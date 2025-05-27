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
    public class AddExistingBookViewModel : BaseViewModel
    {
        public SACH sach { get; set; } = new SACH();
        public PHIEUNHAPSACH pns { get; set; } = new PHIEUNHAPSACH();
        private string _bookText { get; set; }
        public string BookText
        {
            get => _bookText;
            set
            {
                if (_bookText != value)
                {
                    _bookText = value;
                    OnPropertyChanged(nameof(BookText));
                    if (_isSearching) _ = BookSearch();
                }
            }
        }
        public ObservableCollection<SACH> AllBook { get; set; }
        private ObservableCollection<SACH> _listBook { get; set; }
        public ObservableCollection<SACH> ListBook
        {
            get => _listBook;
            set
            {
                if (_listBook != value)
                {
                    _listBook = value;
                    OnPropertyChanged(nameof(ListBook));
                }
            }
        }
        private SACH _bookSelected;
        public SACH BookSelected
        {
            get => _bookSelected;
            set
            {
                _bookSelected = value;
                OnPropertyChanged(nameof(BookSelected));
                if (_bookSelected != null)
                {
                    _isSearching = false;
                    BookText = _bookSelected.DisplayName;
                    _isSearching = true;
                }
            }
        }

        private bool _isSearching = true;
        public Action OnSuccess { get; set; }
        private int tongtien;
        public int TongTien
        {
            get => tongtien;
            set
            {
                if (tongtien != value)
                {
                    tongtien = value;
                    OnPropertyChanged();
                }
            }
        }
        private int soluongnhap;
        public int SoLuongNhap
        {
            get => soluongnhap;
            set
            {
                if (soluongnhap != value)
                {
                    soluongnhap = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<SACH> _bookselectedList { get; set; } = new ObservableCollection<SACH>();
        public ObservableCollection<SACH> BookSelectedList
        {
            get
            {
                return _bookselectedList;
            }
            set
            {
                _bookselectedList = value;
                OnPropertyChanged(nameof(BookSelectedList));
            }
        }
        public SACH SelectedBookInList { get; set; }
        public ICommand AddBookReceivingCM { get; set; }
        public ICommand AddBookToListCM { get; set; }
        public ICommand DeleteBookFromListCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddExistingBookViewModel()
        {
            pns.NgayNhap = DateTime.Now;
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var bookData = await SachBLL.Instance.GetAllSach();
                AllBook = ListBook = new ObservableCollection<SACH>(bookData);
            });
            AddBookReceivingCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                if(BookSelectedList.Count <= 0)
                {
                    MessageBox.Show("Chưa chọn sách nhập");
                    return;
                }
                List<(int, int)> sachnhap = new List<(int, int)>();
                foreach (SACH s in BookSelectedList)
                {
                    sachnhap.Add(((int, int))(s.id, s.SoLuong));
                    bool checkRes = await Task.Run(async () => await SachBLL.Instance.CheckExistingSach(s));
                    if (!checkRes)
                    {
                        MessageBox.Show("Thư viện chưa có sách này, hãy dùng tính năng nhập sách mới để nhập");
                        return;
                    }
                }
                var res = await Task.Run(async () => await PhieuNhapSachBLL.Instance.AddPhieuNhapSach(pns));
                if (!res.Item1)
                {
                    MessageBox.Show(res.Item2);
                    return;
                }
                var addres = await Task.Run(async () => await SachBLL.Instance.AddExistingSach(sachnhap, pns));
                MessageBox.Show(addres.Item2);
                if (addres.Item1)
                {
                    OnSuccess?.Invoke();
                    p.Close();
                }
            });
            AddBookToListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if(BookSelected != null && !BookSelectedList.Any(book => book.id == BookSelected.id) && SoLuongNhap > 0)
                {
                    BookSelected.SoLuong = SoLuongNhap;
                    BookSelectedList.Add(BookSelected);
                    TinhTongTien();
                }
                else
                {
                    if (SoLuongNhap <= 0)
                        MessageBox.Show("Số lượng nhập phải là số dương");
                }
            });
            DeleteBookFromListCM = new RelayCommand<object>((p) => true, (p) =>
            {
                if (SelectedBookInList != null)
                {
                    BookSelectedList.Remove(SelectedBookInList);
                    TinhTongTien();
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
        }
        private async Task BookSearch()
        {
            var res = await SachBLL.Instance.GetSachByMaSach(BookText);
            ListBook = new ObservableCollection<SACH>(res);
        }
        private void TinhTongTien()
        {
            TongTien = 0;
            foreach (SACH s in BookSelectedList)
                if (s.SoLuong > 0 && s.TriGia > 0) TongTien += (int)(s.SoLuong * s.TriGia);
        }

    }
}