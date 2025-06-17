using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Book;
using LibraryManagement.View.Borrow;
using LibraryManagement.View.Reader;
using LibraryManagement.View.Receipt;
using LibraryManagement.View.Report;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand GetNavigationFrameCM { get; set; }
        public ICommand DangXuatCM { get; set; }
        public ICommand ThoatCM { get; set; }

        public ICommand QuanLyDocGiaCM { get; set; }
        public ICommand QuanLySachCM { get; set; }
        public ICommand QuanLyMuonTraCM { get; set; }
        public ICommand QuanLyPhatCM { get; set; }
        public ICommand QuanLyNguoiDungCM { get; set; }
        public ICommand ThongKeCM { get; set; }
        public ICommand QuyDinhCM { get; set; }
        public ICommand TaiKhoanCM { get; set; }
        public ICommand TrangDocGiaCM { get; set; }

        public Frame NavigationFrame { get; set; }

        public ObservableCollection<string> dsChucNangND { get; set; } = new ObservableCollection<string>();

        public bool ShowQuanLyDocGia => dsChucNangND.Contains("QLDG");
        public bool ShowQuanLySach => dsChucNangND.Contains("QLS");
        public bool ShowQuanLyMuonTra => dsChucNangND.Contains("QLPMT");
        public bool ShowQuanLyPhat => dsChucNangND.Contains("QLPT");
        public bool ShowQuanLyNguoiDung => dsChucNangND.Contains("QLND");
        public bool ShowThongKe => dsChucNangND.Contains("BCTK");
        public bool ShowQuyDinh => dsChucNangND.Contains("TDQG");
        public bool ShowTaiKhoan => true;
        public bool ShowTrangDocGia => dsChucNangND.Contains("TTDG");
        public MainViewModel() { }
        public MainViewModel(NGUOIDUNG currentUser)
        {

            GetNavigationFrameCM = new RelayCommand<Frame>((p) => true, (p) => NavigationFrame = p);
            QuanLyDocGiaCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new ReaderTab()));
            QuanLySachCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new BookTab()));
            QuanLyMuonTraCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new BorrowManagement()));
            QuanLyPhatCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new PenaltyReceiptManagement()));
            QuanLyNguoiDungCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new UserTab()));
            ThongKeCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new ReportPage()));
            QuyDinhCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new Rule()));
            TaiKhoanCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new Account()));
            TrangDocGiaCM = new RelayCommand<object>((p) => true, (p) => NavigationFrame.Navigate(new UserReaderTab()));

            TrangDocGiaCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                var w1 = new ReaderTabViewModel(currentUser);
                var v1 = new UserReaderTab() { DataContext = w1 };
                NavigationFrame.Navigate(v1);
            });

            TaiKhoanCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                var w1 = new AccountViewModel(currentUser);
                var v1 = new Account() { DataContext = w1 };
                NavigationFrame.Navigate(v1);
            });

            DangXuatCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                new LoginWindow().Show();
                p.Close();
            });
            GetNavigationFrameCM = new RelayCommand<Frame>((p) => true, (p) =>
            {
                if (p != null)
                    NavigationFrame = p;
            });
            ThoatCM = new RelayCommand<object>((p) => true, (p) => App.Current.Shutdown());

            dsChucNangND.CollectionChanged += (s, e) => NotifyAllPermissionProperties();

            _ = LoadPermissions(currentUser);
        }

        private async Task<List<string>> GetQuyenCuaND(NGUOIDUNG currentUser)
        {
            NHOMNGUOIDUNG nnd = await NhomNguoiDungBLL.Instance.GetNhomNguoiDungById(currentUser.MaNhom);
            return nnd.CHUCNANGs.Select(cn => cn.TenChucNang).ToList();
        }
        private async Task LoadPermissions(NGUOIDUNG currentUser)
        {
            var quyenList = await GetQuyenCuaND(currentUser);
            dsChucNangND.Clear();
            foreach (var q in quyenList)
                dsChucNangND.Add(q);
        }
        private void NotifyAllPermissionProperties()
        {
            OnPropertyChanged(nameof(ShowQuanLyDocGia));
            OnPropertyChanged(nameof(ShowQuanLySach));
            OnPropertyChanged(nameof(ShowQuanLyMuonTra));
            OnPropertyChanged(nameof(ShowQuanLyPhat));
            OnPropertyChanged(nameof(ShowQuanLyNguoiDung));
            OnPropertyChanged(nameof(ShowThongKe));
            OnPropertyChanged(nameof(ShowQuyDinh));
            OnPropertyChanged(nameof(ShowTaiKhoan));
            OnPropertyChanged(nameof(ShowTrangDocGia));
        }

    }
}
