using LibraryManagement.BLL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.GroupUser;
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
    public class UserGroupViewModel : BaseViewModel
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
        private NHOMNGUOIDUNG _nhom { get; set; }
        public NHOMNGUOIDUNG nhom
        {
            get
            {
                return _nhom;
            }
            set
            {
                _nhom = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SearchList { get; set; }
        private ObservableCollection<NHOMNGUOIDUNG> _ListGroupUsers;
        public ObservableCollection<NHOMNGUOIDUNG> ListGroupUsers
        {
            get
            {
                return _ListGroupUsers;
            }
            set
            {
                _ListGroupUsers = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<NHOMNGUOIDUNG> AllGroupUsers { get; set; }
        public NHOMNGUOIDUNG GroupUserSeleted { get; set; }
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataGroupUserCM { get; set; }
        public ICommand SearchGroupUserCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddGroupUserCM { get; set; }
        public ICommand OpenUpdateGroupUserCM { get; set; }
        public ICommand AddNewGroupUserCM { get; set; }
        public ICommand ViewGroupUserCM { get; set; }
        public ICommand UpdateGroupUserCM { get; set; }
        public ICommand DeleteGroupUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public UserGroupViewModel()
        {
            SearchList = new ObservableCollection<string> { "Mã nhóm người dùng" };
            SearchProperties = SearchList.FirstOrDefault();

            LoadDataGroupUserCM = new RelayCommand<NHOMNGUOIDUNG>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await NhomNguoiDungBLL.Instance.GetAllNhomNguoiDung());
                    ListGroupUsers = new ObservableCollection<NHOMNGUOIDUNG>(data);
                    AllGroupUsers = new ObservableCollection<NHOMNGUOIDUNG>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
            SearchGroupUserCM = new RelayCommand<object>((p) => true, async (p) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        ListGroupUsers.Clear();
                        ListGroupUsers = AllGroupUsers;
                    }
                    else
                    {
                        if (SearchProperties == "Mã nhóm người dùng")
                        {
                            var res = await Task.Run(async () => await NhomNguoiDungBLL.Instance.GetNhomNguoiDungByMa(SearchText));
                            ListGroupUsers = new ObservableCollection<NHOMNGUOIDUNG>(res);
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
                ListGroupUsers = new ObservableCollection<NHOMNGUOIDUNG>(AllGroupUsers);
            });
            OpenAddGroupUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                nhom = new NHOMNGUOIDUNG();
                var w1 = new AddGroupUserWindow();
                w1.ShowDialog();
            });
            OpenUpdateGroupUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Window w1 = new EditGroupUserInformationWindow();
                w1.ShowDialog();
            });
            AddNewGroupUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await NhomNguoiDungBLL.Instance.AddNhomNguoiDung(nhom));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataGroupUserCM.Execute(null);
                    p.Close();
                }
            });
            ViewGroupUserCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Window w1 = new GroupUserInformtationWindow();
                w1.ShowDialog();
            });
            UpdateGroupUserCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                var res = await Task.Run(async () => await NhomNguoiDungBLL.Instance.UpdateNhomNguoiDung(GroupUserSeleted));
                MessageBox.Show(res.Item2);
                if (res.Item1)
                {
                    LoadDataGroupUserCM.Execute(null);
                    p.Close();
                }
            });
            DeleteGroupUserCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa nhóm người dùng này không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await NhomNguoiDungBLL.Instance.DeleteNhomNguoiDung(GroupUserSeleted.id));
                    LoadDataGroupUserCM.Execute(null);
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
