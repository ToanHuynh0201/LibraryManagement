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
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand LoginCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        public Action<bool> OnLoginResult { get; set; }
        public Action OnLoginSuccess { get; set; }
        public LoginViewModel()
        {
            LoginCM = new RelayCommand<Window>((p) => true, async (p) =>
            {
                try
                {
                    IsLoading = true;
                    if (string.IsNullOrWhiteSpace(UserName))
                    {
                        MessageBox.Show("Chưa nhập tên đăng nhập!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Chưa nhập mật khẩu!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var dsnd = await NguoiDungBLL.Instance.GetAllUsers();
                    foreach (NGUOIDUNG nd in dsnd)
                    {
                        if (nd.TenDangNhap == UserName && nd.MatKhau == Password)
                        {
                            MessageBox.Show("Đăng nhập thành công", "Thành công",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            Password = "";
                            OnLoginSuccess?.Invoke();
                            OnLoginResult?.Invoke(true);

                            
                            Window w1 = new MainWindow();
                            w1.DataContext = new MainViewModel(nd);
                            w1.Show();

                            p.Close();
                            return;
                        }
                    }

                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!",
                        "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                    OnPropertyChanged(nameof(Password));
                    OnLoginResult?.Invoke(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    OnLoginResult?.Invoke(false);
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }
        private bool CanLogin()
        {
            return !IsLoading &&
                   !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Password);
        }
    }
}