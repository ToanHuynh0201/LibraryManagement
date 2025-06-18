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
    public class AddBookViewModel : BaseViewModel
    {
        public ObservableCollection<int> YearList { get; set; }
        public SACH sach { get; set; } = new SACH();
        public PHIEUNHAPSACH pns { get; set; } = new PHIEUNHAPSACH();
        private string _booktitleText { get; set; }
        public string BookTitleText
        {
            get => _booktitleText;
            set
            {
                if (_booktitleText != value)
                {
                    _booktitleText = value;
                    OnPropertyChanged(nameof(BookTitleText));
                    if (_isSearching) _ = BookTitleSearch();
                }
            }
        }
        public ObservableCollection<DAUSACH> AllBookTitle { get; set; }
        private ObservableCollection<DAUSACH> _listBookTitle { get; set; }
        public ObservableCollection<DAUSACH> ListBookTitle
        {
            get => _listBookTitle;
            set
            {
                if (_listBookTitle != value)
                {
                    _listBookTitle = value;
                    OnPropertyChanged(nameof(ListBookTitle));
                }
            }
        }
        private DAUSACH _booktitleSelected;
        public DAUSACH BookTitleSelected
        {
            get => _booktitleSelected;
            set
            {
                _booktitleSelected = value;
                OnPropertyChanged(nameof(BookTitleSelected));
                if (_booktitleSelected != null)
                {
                    _isSearching = false;
                    BookTitleText = _booktitleSelected.DisplayName;
                    _isSearching = true;
                }
            }
        }

        private bool _isSearching = true;
        public Action OnSuccess { get; set; }
        private string _namxb { get; set; }
        public string namxb
        {
            get => _namxb;
            set
            {
                _namxb = value;
                OnPropertyChanged(nameof(namxb));
            }
        }
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
        private int gianhap;
        public int GiaNhap
        {
            get => gianhap;
            set
            {
                if (gianhap != value)
                {
                    gianhap = value;
                    OnPropertyChanged();
                    TinhTongTien();
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
                    TinhTongTien();
                }
            }
        }
        public ICommand AddNewBookCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public ICommand LoadOtherDataCM { get; set; }
        public AddBookViewModel()
        {
            pns.NgayNhap = DateTime.Now;
            YearList = new ObservableCollection<int>(Enumerable.Range(2000, DateTime.Now.Year - 2000 + 1).Reverse());
            LoadOtherDataCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                var booktitleData = await DauSachBLL.Instance.GetAllDauSach();
                AllBookTitle = ListBookTitle = new ObservableCollection<DAUSACH>(booktitleData);
            });
            AddNewBookCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                bool checkRes = await Task.Run(async () => await SachBLL.Instance.CheckExistingSach(sach));
                if(checkRes)
                {
                    MessageBox.Show("Thư viện đã có sách này, hãy dùng tính năng nhập sách đã có");
                    return;
                }
                var res = await Task.Run(async () => await PhieuNhapSachBLL.Instance.AddPhieuNhapSach(pns));
                if (!res.Item1)
                {
                    MessageBox.Show(res.Item2);
                    return;
                }    
                sach.MaDauSach = (BookTitleSelected == null) ? 0 : BookTitleSelected.id;
                sach.NamXB = Convert.ToInt32(namxb);
                sach.TriGia = GiaNhap;
                sach.SoLuong = SoLuongNhap;

                var addres = await Task.Run(async () => await SachBLL.Instance.AddNewSach(sach, pns));
                MessageBox.Show(addres.Item2);

                if (addres.Item1)
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
        private async Task BookTitleSearch()
        {
            var res = await DauSachBLL.Instance.GetDauSachByTen(BookTitleText);
            ListBookTitle = new ObservableCollection<DAUSACH>(res);
        }
        private void TinhTongTien()
        {
            if (GiaNhap > 0 && SoLuongNhap > 0)
            {
                TongTien = GiaNhap * SoLuongNhap;
            }
            else
            {
                TongTien = 0;
            }
        }

    }
}