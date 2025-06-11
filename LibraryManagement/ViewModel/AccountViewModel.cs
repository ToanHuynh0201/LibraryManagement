using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class AccountViewModel : BaseViewModel
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
        public ObservableCollection<string> SearchList { get; set; }
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
        public NGUOIDUNG UserSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataUserCM { get; set; }
        public ICommand SearchUserCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddUserCM { get; set; }
        public ICommand OpenUpdateUserCM { get; set; }
        public ICommand AddNewUserCM { get; set; }
        public ICommand ViewUserCM { get; set; }
        public ICommand UpdateUserCM { get; set; }
        public ICommand DeleteUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public AccountViewModel()
        {

            SearchList = new ObservableCollection<string> { "Username" };
            SearchProperties = SearchList.FirstOrDefault();

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
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListUsers.Clear();
                        ListUsers = AllUsers;
                    }
                    else
                    {
                        if (SearchProperties == "Username")
                        {
                            nguoidung = await Task.Run(async () => await NguoiDungBLL.Instance.GetNguoiDungByTenDN(SearchText));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            });
            ResetDataCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SearchText = "";
                ListUsers = new ObservableCollection<NGUOIDUNG>(AllUsers);
            });
            //OpenAddUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            //{
            //    nguoidung = new NGUOIDUNG()
            //    {
            //        MaNhom = 1
            //    };
            //    var w1 = new AddUserWindow();
            //    w1.ShowDialog();
            //});
            //OpenUpdateUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            //{
            //    Window w1 = new EditUserInformationWindow();
            //    w1.ShowDialog();
            //});
            AddNewUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await NguoiDungBLL.Instance.AddNguoiDung(nguoidung));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataUserCM.Execute(null);
                    p.Close();
                }
            });
            //ViewUserCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            //{
            //    Window w1 = new UserInformationWindow();
            //    w1.ShowDialog();
            //});
            UpdateUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await NguoiDungBLL.Instance.UpdateNguoiDung(UserSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataUserCM.Execute(null);
                    p.Close();
                }
            });
            DeleteUserCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa người dùng này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await NguoiDungBLL.Instance.DeleteNguoiDung(UserSeleted.TenDangNhap));
                    LoadDataUserCM.Execute(null);
                    MessageBox.Show(res.Item2);
                }
            });
            CloseWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
