using LibraryManagement.BLL;
using LibraryManagement.DTO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AccountViewModel : BaseViewModel
    {
        public NGUOIDUNG CurrentUser { get; set; }

        private string _tenNguoiDung;
        public string TenNguoiDung
        {
            get => _tenNguoiDung;
            set { _tenNguoiDung = value; OnPropertyChanged(); }
        }

        private string _tenTaiKhoan;
        public string TenTaiKhoan
        {
            get => _tenTaiKhoan;
            set { _tenTaiKhoan = value; OnPropertyChanged(); }
        }

        private string _tenNhom;
        public string TenNhom
        {
            get => _tenNhom;
            set { _tenNhom = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CHUCNANG> DanhSachChucNang { get; set; } = new ObservableCollection<CHUCNANG>();

        public ICommand LoadDataCM { get; set; }
        public ICommand ChangePasswordCM { get; set; }

        public AccountViewModel() { }
        public AccountViewModel(NGUOIDUNG user)
        {
            LoadDataCM = new RelayCommand<object>((p) => true, (p) =>
            {
                CurrentUser = user;
                OnPropertyChanged(nameof(CurrentUser));
                LoadThongTinNguoiDung();
                LoadChucNangNguoiDung();
            });
            ChangePasswordCM = new RelayCommand<object>((p) => true, (p) =>
            {
                MessageBox.Show("Tính năng chưa làm", "Thông báo");
            });
        }

        private void LoadThongTinNguoiDung()
        {
            TenNguoiDung = CurrentUser.TenNguoiDung;
            TenTaiKhoan = CurrentUser.TenDangNhap;
            TenNhom = CurrentUser.NHOMNGUOIDUNG?.TenNhom ?? "";
        }

        private async void LoadChucNangNguoiDung()
        {
            var nhom = await NhomNguoiDungBLL.Instance.GetNhomNguoiDungById(CurrentUser.MaNhom);
            if (nhom != null)
            {
                DanhSachChucNang.Clear();
                foreach (var cn in nhom.CHUCNANGs)
                    DanhSachChucNang.Add(cn);
            }
        }
    }
}
