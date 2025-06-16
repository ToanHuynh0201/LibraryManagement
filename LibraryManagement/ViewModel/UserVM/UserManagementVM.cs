using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Users;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class UserManagementViewModel : BaseViewModel
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



        private NGUOIDUNG _nguoidung { get; set; }
        public NGUOIDUNG nguoidung
        {
            get
            {
                return _nguoidung;
            }
            set
            {
                _nguoidung = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<NGUOIDUNG> _ListUsers;
        public ObservableCollection<NGUOIDUNG> ListUsers
        {
            get
            {
                return _ListUsers;
            }
            set
            {
                _ListUsers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NGUOIDUNG> AllUsers { get; set; }
        public NGUOIDUNG UserSelected { get; set; }
        private bool _isSearching = true;
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataUserCM { get; set; }
        public ICommand SearchUserCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddUserCM { get; set; }
        public ICommand OpenUpdateUserCM { get; set; }
        public ICommand ViewUserCM { get; set; }
        public ICommand DeleteUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public UserManagementViewModel()
        {
            LoadDataUserCM = new RelayCommand<NGUOIDUNG>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await NguoiDungBLL.Instance.GetAllUsers());
                    ListUsers = new ObservableCollection<NGUOIDUNG>(data);
                    AllUsers = new ObservableCollection<NGUOIDUNG>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });

            SearchUserCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                await SearchUsers();
            });

            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListUsers = new ObservableCollection<NGUOIDUNG>(AllUsers);
            });

            OpenAddUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddUserViewModel();
                vm1.OnSuccess = () => LoadDataUserCM.Execute(null);
                var w1 = new AddUserWindow { DataContext = vm1 };
                w1.ShowDialog();
            });

            OpenUpdateUserCM = new RelayCommand<Window>((p) => UserSelected != null, (p) =>
            {
                var vm1 = new UpdateUserViewModel(UserSelected);
                vm1.OnSuccess = () => LoadDataUserCM.Execute(null);
                var w1 = new EditUserWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });

            DeleteUserCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa người dùng không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await NguoiDungBLL.Instance.DeleteNguoiDung(UserSelected.TenDangNhap));
                    LoadDataUserCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });

            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        private async Task SearchUsers()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    ListUsers = AllUsers;
                    return;
                }

                var res = AllUsers.Where(x =>
                    (!string.IsNullOrEmpty(x.TenNguoiDung) && x.TenNguoiDung.Contains(SearchText) ||
                    (!string.IsNullOrEmpty(x.TenDangNhap) && x.TenDangNhap.Contains(SearchText)))).ToList();

                ListUsers = new ObservableCollection<NGUOIDUNG>(res);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }
    }
}