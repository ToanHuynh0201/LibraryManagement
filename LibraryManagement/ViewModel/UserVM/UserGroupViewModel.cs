using LibraryManagement.BLL;
using LibraryManagement.DAL;
using LibraryManagement.DTO;
using LibraryManagement.View;
using LibraryManagement.View.GroupUser;
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
    public class UserGroupViewModel : BaseViewModel
    {
        #region Properties

        private NHOMNGUOIDUNG _nhomnguoidung { get; set; }
        public NHOMNGUOIDUNG nhomnguoidung
        {
            get
            {
                return _nhomnguoidung;
            }
            set
            {
                _nhomnguoidung = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<NHOMNGUOIDUNG> _ListGroupGroupUser;
        public ObservableCollection<NHOMNGUOIDUNG> ListGroupGroupUser
        {
            get
            {
                return _ListGroupGroupUser;
            }
            set
            {
                _ListGroupGroupUser = value;
                OnPropertyChanged();
            }
        }
        public NHOMNGUOIDUNG GroupUserSelected { get; set; }
        private bool _isSearching = true;
        #endregion

        #region Commands
        public ICommand GetCurrentWindowCM { get; set; }
        public ICommand LoadDataGroupUserCM { get; set; }
        public ICommand SearchGroupUserCM { get; set; }
        public ICommand ResetDataCM { get; set; }
        public ICommand OpenAddGroupUserCM { get; set; }
        public ICommand OpenUpdateGroupUserCM { get; set; }
        public ICommand ViewGroupUserCM { get; set; }
        public ICommand DeleteGroupUserCM { get; set; }
        public ICommand CloseWindowCM { get; set; }
        #endregion

        public UserGroupViewModel()
        {
            LoadDataGroupUserCM = new RelayCommand<NHOMNGUOIDUNG>((p) => { return true; }, async (p) =>
            {
                try
                {
                    var data = await Task.Run(async () => await NhomNguoiDungBLL.Instance.GetAllNhomNguoiDung());
                    ListGroupGroupUser = new ObservableCollection<NHOMNGUOIDUNG>(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });

            OpenAddGroupUserCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                var vm1 = new AddGroupUserViewModel();
                vm1.OnSuccess = () => LoadDataGroupUserCM.Execute(null);
                var w1 = new AddGroupUserWindow { DataContext = vm1 };
                w1.ShowDialog();
            });

            OpenUpdateGroupUserCM = new RelayCommand<Window>((p) => GroupUserSelected != null, (p) =>
            {
                var vm1 = new EditGroupUserViewModel(GroupUserSelected);
                vm1.OnSuccess = () => LoadDataGroupUserCM.Execute(null);
                var w1 = new EditGroupUserWindow() { DataContext = vm1 };
                w1.ShowDialog();
            });

            DeleteGroupUserCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa nhóm nhóm người dùng không?", "Xác nhận xóa",
                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var res = await Task.Run(async () => await NhomNguoiDungBLL.Instance.DeleteNhomNguoiDung(GroupUserSelected.id));
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