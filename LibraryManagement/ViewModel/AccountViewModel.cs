using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View.Users;
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

        private string _newPassword1;
        public string NewPassword1
        {
            get => _newPassword1;
            set { _newPassword1 = value; OnPropertyChanged(); }
        }

        private string _newPassword2;
        public string NewPassword2
        {
            get => _newPassword2;
            set { _newPassword2 = value; OnPropertyChanged(); }
        }

        public ICommand SaveNewPasswordCM { get; set; }
        public ICommand LoadDataCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
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
                NewPassword1 = NewPassword2 = "";
                var w1 = new ForgetPasswordWindow() { DataContext = this };
                w1.ShowDialog();
            });
            CloseWindowCM = new RelayCommand<Window>((p) => true, (p) =>
            {
                p.Close();
            });
            SaveNewPasswordCM = new RelayCommand<Window>((p) => true, async (p) => await SaveNewPasswordAsync(p));
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
        private async Task SaveNewPasswordAsync(Window window)
        {
            if(string.IsNullOrWhiteSpace(NewPassword1) || string.IsNullOrWhiteSpace(NewPassword2))
            {
                MessageBox.Show("Mật khẩu không được để trống");
                return;
            }
            if (NewPassword1 != NewPassword2)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentUser.MatKhau = NewPassword1;
            var res = await NguoiDungBLL.Instance.ChangePassWord(CurrentUser);

            if (res.Item1)
            {
                MessageBox.Show(res.Item2, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                window?.Close();
            }
            else
            {
                MessageBox.Show(res.Item2, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
